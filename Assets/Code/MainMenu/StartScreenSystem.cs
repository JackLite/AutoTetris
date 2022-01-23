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
        private StartCoreSettings startCoreSettings;
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
            startCoreSettings.isContinue = true;
            startCoreSettings.isDebug = false;
            Start();
        }

        private void StartGame()
        {
            startCoreSettings.isDebug = false;
            Start();
        }

        private void StartDebug()
        {
            startCoreSettings.isDebug = true;
            Start();
        }

        private void Start()
        {
            Addressables.ReleaseInstance(_startScreenMono.gameObject);
            _eventTable.AddEvent<StartCoreSignal>();
        }
    }
}