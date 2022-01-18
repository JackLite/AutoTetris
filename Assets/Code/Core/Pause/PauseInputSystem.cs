using Core.Pause.Signals;
using EcsCore;
using Leopotam.Ecs;
using UnityEngine;

namespace Core.Pause
{
    [EcsSystem(typeof(CoreModule))]
    public class PauseInputSystem : IEcsInitSystem, IEcsDestroySystem
    {
        private MainScreenMono _mainScreenMono;
        private EcsWorld _world;
        private EcsEventTable _eventTable;

        public void Init()
        {
            _mainScreenMono.PauseButton.onClick.AddListener(OnPause);
            _mainScreenMono.UnPauseButton.onClick.AddListener(OnUnPause);
        }

        private void OnPause()
        {
            _eventTable.AddEvent<PauseSignal>();
            _eventTable.AddEvent<ShowPauseScreenSignal>();
        }

        private void OnUnPause()
        {
            _eventTable.AddEvent<UnpauseSignal>();
            _eventTable.AddEvent<HidePauseScreenSignal>();
        }

        public void Destroy()
        {
            _mainScreenMono.PauseButton.onClick.RemoveListener(OnPause);
            _mainScreenMono.UnPauseButton.onClick.AddListener(OnUnPause);

            Time.timeScale = 1;
        }
    }
}