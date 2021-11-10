using Core;
using EcsCore;
using Leopotam.Ecs;
using UnityEngine.AddressableAssets;

namespace Global
{
    [EcsSystem(typeof(MainModule))]
    public class StartScreenSystem : IEcsInitSystem, IEcsRunSystem
    {
        private StartScreenMono _startScreenMono;
        private EcsWorld _world;
        private EcsFilter<StartGameSignal> _filter;
        public void Init()
        {
            _startScreenMono.StartGameButton.OnClick += StartGame;
        }

        private void StartGame()
        {
            Addressables.ReleaseInstance(_startScreenMono.gameObject);
            _world.NewEntity().Replace(new EcsModuleActivationSignal {ModuleType = typeof(CoreModule)});
        }

        public void Run()
        {
            if (_filter.GetEntitiesCount() == 0)
                return;
            
            _world.NewEntity().Replace(new EcsModuleActivationSignal {ModuleType = typeof(CoreModule)});

            _filter.GetEntity(0).Destroy();
        }
    }
}