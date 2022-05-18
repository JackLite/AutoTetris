using Core.Pause.Signals;
using EcsCore;
using Global.Audio;
using Global.Saving;
using Global.Settings;
using Leopotam.Ecs;

namespace Core.Pause
{
    [EcsSystem(typeof(CoreModule))]
    public class PauseScreenSystem : IEcsInitSystem, IEcsRunSystem, IEcsRunLateSystem
    {
        private EcsEventTable _eventTable;
        private EcsFilter<AudioChangeEvent> _audioChangeFilter;
        private EcsWorld _world;
        private GlobalSettings _globalSettings;
        private PauseScreenView _pauseScreen;
        private SaveService _saveService;

        public void Init()
        {
            InitAudioButtons();
        }

        public void Run()
        {
            if (_eventTable.Has<ShowPauseScreenSignal>())
                _pauseScreen.gameObject.SetActive(true);

            if (_eventTable.Has<HidePauseScreenSignal>())
                _pauseScreen.gameObject.SetActive(false);
        }

        private void InitAudioButtons()
        {
            _pauseScreen.AudioSettingsView.MusicButton.SetState(_saveService.GetMusicState());
            _pauseScreen.AudioSettingsView.SoundButton.SetState(_saveService.GetSoundState());

            _pauseScreen.AudioSettingsView.MusicButton.OnClick += () => OnAudioButtonClick(true);

            _pauseScreen.AudioSettingsView.SoundButton.OnClick += () => OnAudioButtonClick();
        }

        private void OnAudioButtonClick(bool isMusic = false)
        {
            _world.CreateOneFrame().Replace(AudioHelper.Create(_globalSettings, AudioEnum.GUIButton));
            if (isMusic)
                AudioHelper.CreateChangeMusicState(_world, _saveService);
            else
                AudioHelper.CreateChangeSoundState(_world, _saveService);
        }
        
        public void RunLate()
        {
            foreach (var i in _audioChangeFilter)
            {
                ref var changeEvent = ref _audioChangeFilter.Get1(i);
                if (changeEvent.isMusic)
                    _pauseScreen.AudioSettingsView.MusicButton.SetState(changeEvent.isActive);
                else
                    _pauseScreen.AudioSettingsView.SoundButton.SetState(changeEvent.isActive);
            }
        }
    }
}