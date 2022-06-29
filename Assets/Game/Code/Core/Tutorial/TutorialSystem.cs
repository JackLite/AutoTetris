using Core.Input;
using Core.Moving;
using EcsCore;
using Global;
using Global.Saving;
using Leopotam.Ecs;
using UnityEngine;

namespace Core.Tutorial
{
    [EcsSystem(typeof(TutorialModule))]
    public class TutorialSystem : IEcsInitSystem, IEcsRunSystem, IEcsDestroySystem
    {
        private EcsWorld _world;
        private EcsEventTable _eventTable;
        private PlayerData _playerData;
        private SaveService _saveService;
        private MainScreenMono _mainScreen;
        private EcsOneData<TutorialProgressData> _tutorProgressData;
        private MovingData _movingData;

        public void Init()
        {
            _mainScreen.Tutorial.gameObject.SetActive(true);
            _mainScreen.Tutorial.ShowArrow(Direction.Right, _tutorProgressData.GetData().delay);
            EcsWorldEventsBlackboard.AddEventHandler<InputRawEvent>(OnInputEvent);
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

            data.isWaitSwipe = false;
            data.delay = 2;
            _movingData.isMoveAllowed = true;
            _world.NewEntity().Replace(new SwipeInput { direction = ev.direction });
            data.step++;

            if (data.step == 1)
                _mainScreen.Tutorial.ShowArrow(Direction.Left, 2);
            else if (data.step == 2)
                _mainScreen.Tutorial.ShowArrow(Direction.Bottom, 1);
            else
            {
                _saveService.SaveTutorCompleted();
                _mainScreen.Tutorial.gameObject.SetActive(false);
                _world.DeactivateModule<TutorialModule>();
            }
        }

        public void Destroy()
        {
            EcsWorldEventsBlackboard.RemoveEventHandler<InputRawEvent>(OnInputEvent);
        }
    }
}