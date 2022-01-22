using EcsCore;
using Global;
using Global.Saving;
using Leopotam.Ecs;
using UnityEngine.AddressableAssets;

namespace MainMenu
{
    [EcsSystem(typeof(MainModule))]
    public class StartScreenSystem : IEcsInitSystem
    {
        private StartScreenMono _startScreenMono;
        private EcsWorld _world;
        private EcsEventTable _eventTable;
        private CoreConfig _coreConfig;
        private SaveService _saveService;

        public void Init()
        {
            _startScreenMono.StartGameButton.OnClick += StartGame;
            _startScreenMono.ContinueGameButton.onClick.AddListener(ContinueGame);
            _startScreenMono.StartDebugButton.onClick.AddListener(StartDebug);
            _startScreenMono.ContinueGameButton.gameObject.SetActive(_saveService.HasGame());
        }
        private void ContinueGame()
        {
            _coreConfig.isContinue = true;
            _coreConfig.isDebug = false;
            Start();
        }

        private void StartGame()
        {
            _coreConfig.isDebug = false;
            Start();
        }

        private void StartDebug()
        {
            _coreConfig.isDebug = true;
            Start();
        }

        private void Start()
        {
            Addressables.ReleaseInstance(_startScreenMono.gameObject);
            _eventTable.AddEvent<StartCoreSignal>();
        }
    }
}