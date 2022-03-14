using Core.Pause.Signals;
using EcsCore;
using Global.Audio;
using Global.Settings;
using Global.Settings.Audio;
using Leopotam.Ecs;
using UnityEngine;

namespace Core.Pause
{
    [EcsSystem(typeof(CoreModule))]
    public class PauseInputSystem : IEcsInitSystem, IEcsDestroySystem
    {
        private EcsEventTable _eventTable;
        private EcsWorld _world;
        private GlobalSettings _settings;
        private MainScreenMono _mainScreenMono;

        public void Init()
        {
            _mainScreenMono.PauseButton.onClick.AddListener(OnPause);
            _mainScreenMono.UnPauseButton.onClick.AddListener(OnUnPause);
        }

        private void OnPause()
        {
            _world.CreateOneFrame().Replace(AudioHelper.Create(_settings, AudioEnum.GUIButton));
            _eventTable.AddEvent<PauseSignal>();
            _eventTable.AddEvent<ShowPauseScreenSignal>();
        }

        private void OnUnPause()
        {
            _world.CreateOneFrame().Replace(AudioHelper.Create(_settings, AudioEnum.GUIButton));
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