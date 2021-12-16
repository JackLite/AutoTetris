using Core;
using EcsCore;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Global.GameOver
{
    [EcsSystem(typeof(MainModule))]
    public class GameOverSystem : IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsEventTable _eventTable;
        private GameObject _gameOverScreen;

        public void Run()
        {
            if (!_eventTable.IsEventExist<GameOverSignal>())
                return;

            CreateGameOverScreen();
            _world.NewEntity()
                  .Replace(new EcsModuleDeactivationSignal
                  {
                      ModuleType = typeof(CoreModule)
                  });
            
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
            _world.ActivateModule<CoreModule>();
            Addressables.ReleaseInstance(_gameOverScreen);
        }
    }
}