using System.Linq;
using EcsCore;
using Global;
using Global.Audio;
using Global.Saving;
using Global.Settings;
using Global.Settings.Audio;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace MainMenu
{
    [EcsSystem(typeof(MainModule))]
    public class StartScreenSystem : IEcsInitSystem
    {
        private EcsWorld _world;
        private EcsEventTable _eventTable;
        private GlobalSettings _settings;
        private StartCoreData startCoreData;
        private StartScreenMono _startScreenMono;
        private SaveService _saveService;

        public void Init()
        {
            _startScreenMono.StartGameButton.OnClick += StartGame;
            //_startScreenMono.ContinueGameButton.onClick.AddListener(ContinueGame);
            _startScreenMono.StartDebugButton.gameObject.SetActive(Debug.isDebugBuild);
            _startScreenMono.StartDebugButton.onClick.AddListener(StartDebug);
            //_startScreenMono.ContinueGameButton.gameObject.SetActive(_saveService.HasGame());
        }
        private void ContinueGame()
        {
            _world.CreateOneFrame().Replace(AudioHelper.Create(_settings, AudioEnum.GUIButton));
            startCoreData.isContinue = true;
            startCoreData.isDebug = false;
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
            startCoreData.isDebug = false;
            Start();
        }

        private void StartDebug()
        {
            _world.CreateOneFrame().Replace(AudioHelper.Create(_settings, AudioEnum.GUIButton));
            startCoreData.isDebug = true;
            Start();
        }

        private void Start()
        {
            Addressables.ReleaseInstance(_startScreenMono.gameObject);
            _eventTable.AddEvent<StartCoreSignal>();
        }
    }
}