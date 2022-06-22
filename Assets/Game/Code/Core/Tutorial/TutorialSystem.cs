using Core.Input;
using EcsCore;
using Global;
using Global.Saving;
using Leopotam.Ecs;

namespace Core.Tutorial
{
    [EcsSystem(typeof(TutorialModule))]
    public class TutorialSystem : IEcsInitSystem, IEcsDestroySystem
    {
        private EcsWorld _world;
        private PlayerData _playerData;
        private SaveService _saveService;
        private MainScreenMono _mainScreen;
        private EcsOneData<TutorialProgressData> _tutorProgressData;

        public void Init()
        {
            _mainScreen.Tutorial.gameObject.SetActive(true);
            _mainScreen.Tutorial.ShowArrow(Direction.Right);
            EcsWorldEventsBlackboard.AddEventHandler<InputRawEvent>(OnInputEvent);
        }

        private void OnInputEvent(InputRawEvent ev)
        {
            ref var data = ref _tutorProgressData.GetData();
            if (ev.direction != Direction.Right && data.step == 0)
                return;
            if (ev.direction != Direction.Left && data.step == 1)
                return;
            if (ev.direction != Direction.Bottom && data.step == 2)
                return;

            _world.NewEntity().Replace(new SwipeInput { direction = ev.direction });
            data.step++;

            if (data.step == 1)
                _mainScreen.Tutorial.ShowArrow(Direction.Left);
            else if (data.step == 2)
                _mainScreen.Tutorial.ShowArrow(Direction.Bottom);
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