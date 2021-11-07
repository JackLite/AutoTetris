using Core;
using EcsCore;
using Leopotam.Ecs;
using UnityEngine.AddressableAssets;

namespace Global
{
    [EcsSystem(typeof(StartScreenModule))]
    public class StartScreenSystem : IEcsInitSystem
    {
        private StartScreenMono _startScreenMono;
        private EcsWorld _world;
        public void Init()
        {
            _startScreenMono.StartGameButton.OnClick += StartGame;
        }

        private void StartGame()
        {
            Addressables.ReleaseInstance(_startScreenMono.gameObject);
            _world.NewEntity().Replace(new EcsModuleActivationSignal {ModuleType = typeof(CoreModule)});
        }
    }
}