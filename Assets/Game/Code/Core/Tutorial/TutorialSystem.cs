using System;
using System.Threading.Tasks;
using Core.Cells.Visual;
using Core.Input;
using Core.Moving;
using EcsCore;
using Global;
using Global.Analytics;
using Global.Saving;
using Leopotam.Ecs;
using UnityEngine;

namespace Core.Tutorial
{
    [EcsSystem(typeof(TutorialModule))]
    public class TutorialSystem : IEcsInitSystem, IEcsRunSystem, IEcsDestroySystem
    {
        private CellsViewProvider _cellsViewProvider;
        private EcsEventTable _eventTable;
        private EcsOneData<TutorialProgressData> _tutorProgressData;
        private EcsWorld _world;
        private MainScreenMono _mainScreen;
        private MovingData _movingData;
        private PlayerData _playerData;
        private SaveService _saveService;

        public void Init()
        {
            ShowStep(0, _tutorProgressData.GetData().delay);
            EcsWorldEventsBlackboard.AddEventHandler<InputRawEvent>(OnInputEvent);
            _world.CreateOneFrame().Replace(new AnalyticEvent { eventId = "tutorial_step", data = "0" });
        }

        public void Run()
        {
            ref var data = ref _tutorProgressData.GetData();
            if (data.isWaitSwipe)
                return;
            if (data.delay <= 0)
            {
                _movingData.isMoveAllowed = false;
                data.isWaitSwipe = true;
            }
            else
            {
                data.delay -= Time.deltaTime;
            }
        }

        private void OnInputEvent(InputRawEvent ev)
        {
            ref var data = ref _tutorProgressData.GetData();
            if (!data.isWaitSwipe)
                return;
            if (ev.direction != Direction.Right && data.step == 0)
                return;
            if (ev.direction != Direction.Left && data.step == 1)
                return;
            if (ev.direction != Direction.Bottom && data.step == 2)
                return;

            _mainScreen.Tutorial.gameObject.SetActive(false);
            data.isWaitSwipe = false;
            data.delay = 2;
            _movingData.isMoveAllowed = true;
            _world.NewEntity().Replace(new SwipeInput { direction = ev.direction });
            data.step++;
            _world.CreateOneFrame()
                  .Replace(new AnalyticEvent { eventId = "tutorial_step", data = data.step.ToString() });

            if (data.step == 1)
            {
                ShowStep(1, 2);
                _mainScreen.ShadowCellsController.ResetRightParent();
                foreach (var cellMono in _cellsViewProvider.GetLightedCells(Direction.Right))
                    cellMono.ResetLayer();
            }
            else if (data.step == 2)
            {
                ShowStep(2, 2);
                _mainScreen.ShadowCellsController.ResetLeftParent();
                foreach (var cellMono in _cellsViewProvider.GetLightedCells(Direction.Left))
                    cellMono.ResetLayer();
            }
            else
            {
                _mainScreen.ShadowCellsController.ResetBottomParent();
                foreach (var cellMono in _cellsViewProvider.GetLightedCells(Direction.Bottom))
                    cellMono.ResetLayer();
                _saveService.SaveTutorCompleted();
                _mainScreen.Tutorial.gameObject.SetActive(false);
                _world.DeactivateModule<TutorialModule>();
            }
        }

        private async void ShowStep(int stepNumber, float delay)
        {
            await Task.Delay(TimeSpan.FromSeconds(delay));
            _mainScreen.Tutorial.gameObject.SetActive(true);
            if (stepNumber == 0)
            {
                _mainScreen.Tutorial.ShowArrow(Direction.Right);
                _mainScreen.ShadowCellsController.LightUpRight();
                foreach (var cellMono in _cellsViewProvider.GetLightedCells(Direction.Right))
                    cellMono.MoveToTopLayer();
            }
            else if (stepNumber == 1)
            {
                _mainScreen.Tutorial.ShowArrow(Direction.Left);
                _mainScreen.ShadowCellsController.LightUpLeft();
                foreach (var cellMono in _cellsViewProvider.GetLightedCells(Direction.Left))
                    cellMono.MoveToTopLayer();
            }
            else if (stepNumber == 2)
            {
                _mainScreen.Tutorial.ShowArrow(Direction.Bottom);
                _mainScreen.ShadowCellsController.LightUpBottom();
                foreach (var cellMono in _cellsViewProvider.GetLightedCells(Direction.Bottom))
                    cellMono.MoveToTopLayer();
            }
        }

        public void Destroy()
        {
            EcsWorldEventsBlackboard.RemoveEventHandler<InputRawEvent>(OnInputEvent);
        }
    }
}