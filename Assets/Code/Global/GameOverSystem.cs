using Core;
using EcsCore;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Global
{
    [EcsSystem(typeof(MainModule))]
    public class GameOverSystem : IEcsRunSystem
    {
        private EcsFilter<GameOverSignal> _filter;
        private EcsWorld _world;
        private GameObject _gameOverScreen;

        public void Run()
        {
            if (_filter.GetEntitiesCount() == 0)
                return;

            CreateGameOverScreen();
            _world.NewEntity()
                  .Replace(new EcsModuleDeactivationSignal
                  {
                      ModuleType = typeof(CoreModule)
                  });
            
            _filter.GetEntity(0).Destroy();
        }

        private async void CreateGameOverScreen()
        {
            var handle = Addressables.InstantiateAsync("GameOverScreen");
            await handle.Task;
            _gameOverScreen = handle.Result;
            _gameOverScreen.GetComponent<GameOverMono>().OnTryAgain += StartGame;
        }

        private void StartGame()
        {
            _world.NewEntity().Replace(new StartGameSignal());
            Addressables.ReleaseInstance(_gameOverScreen);
        }
    }
}