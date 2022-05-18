using EcsCore;
using Global;
using Global.Audio;
using Global.Saving;
using Global.Settings;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace MainMenu
{
    [EcsSystem(typeof(MainMenuModule))]
    public class StartScreenSystem : IEcsInitSystem, IEcsRunLateSystem
    {
        private EcsWorld _world;
        private EcsEventTable _eventTable;
        private GlobalSettings _settings;
        private StartCoreData _startCoreData;
        private StartScreenMono _startScreen;
        private SaveService _saveService;

        private EcsFilter<AudioChangeEvent> _audioChangeFilter;

        public void Init()
        {
            _startScreen.StartGameButton.OnClick += StartGame;
            InitAudioButtons();
            //_startScreenMono.ContinueGameButton.onClick.AddListener(ContinueGame);
            _startScreen.StartDebugButton.gameObject.SetActive(Debug.isDebugBuild);
            _startScreen.StartDebugButton.onClick.AddListener(StartDebug);
            //_startScreenMono.ContinueGameButton.gameObject.SetActive(_saveService.HasGame());
        }
        private void InitAudioButtons()
        {
            _startScreen.AudioSettingsView.MusicButton.SetState(_saveService.GetMusicState());
            _startScreen.AudioSettingsView.SoundButton.SetState(_saveService.GetSoundState());

            _startScreen.AudioSettingsView.MusicButton.OnClick += () => OnAudioButtonClick(true);

            _startScreen.AudioSettingsView.SoundButton.OnClick += () => OnAudioButtonClick();
        }

        private void OnAudioButtonClick(bool isMusic = false)
        {
            _world.CreateOneFrame().Replace(AudioHelper.Create(_settings, AudioEnum.GUIButton));
            if (isMusic)
                AudioHelper.CreateChangeMusicState(_world, _saveService);
            else
                AudioHelper.CreateChangeSoundState(_world, _saveService);
        }
        
        private void ContinueGame()
        {
            _world.CreateOneFrame().Replace(AudioHelper.Create(_settings, AudioEnum.GUIButton));
            _startCoreData.isContinue = true;
            _startCoreData.isDebug = false;
            Start();
        }

        private void StartGame()
        {
            if (_saveService.HasGame())
            {
                ContinueGame();
                return;
            }
            _world.CreateOneFrame().Replace(AudioHelper.Create(_settings, AudioEnum.GUIButton));
            _startCoreData.isDebug = false;
            Start();
        }

        private void StartDebug()
        {
            _world.CreateOneFrame().Replace(AudioHelper.Create(_settings, AudioEnum.GUIButton));
            _startCoreData.isDebug = true;
            Start();
        }

        private void Start()
        {
            Addressables.ReleaseInstance(_startScreen.gameObject);
            _eventTable.AddEvent<StartCoreSignal>();
            _world.DeactivateModule<MainMenuModule>();
        }

        public void RunLate()
        {
            foreach (var i in _audioChangeFilter)
            {
                ref var changeEvent = ref _audioChangeFilter.Get1(i);
                if (changeEvent.isMusic)
                    _startScreen.AudioSettingsView.MusicButton.SetState(changeEvent.isActive);
                else
                    _startScreen.AudioSettingsView.SoundButton.SetState(changeEvent.isActive);
            }
        }
    }
}