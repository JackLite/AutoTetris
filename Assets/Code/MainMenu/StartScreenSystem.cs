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
            _startScreenMono.ContinueGameButton.onClick.AddListener(StartDebug);
            _startScreenMono.StartDebugButton.onClick.AddListener(ContinueGame);
            if (_saveService.HasGame())
                _startScreenMono.ContinueGameButton.gameObject.SetActive(true);
        }
        private void ContinueGame()
        {
            Addressables.ReleaseInstance(_startScreenMono.gameObject);
            _world.NewEntity().Replace(new StartCoreComponent { isContinue = true });
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
            _world.NewEntity().Replace(new StartCoreComponent());
        }
    }
}