using EcsCore;
using Global;
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

        public void Init()
        {
            _startScreenMono.StartGameButton.OnClick += StartGame;
            _startScreenMono.StartDebugButton.onClick.AddListener(StartDebug);
        }

        private void StartGame()
        {
            _coreConfig.IsDebug = false;
            Start();
        }

        private void StartDebug()
        {
            _coreConfig.IsDebug = true;
            Start();
        }

        private void Start()
        {
            Addressables.ReleaseInstance(_startScreenMono.gameObject);
            _eventTable.AddEvent<StartCoreSignal>();
        }
    }
}