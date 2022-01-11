using Core;
using Core.Ads;
using Core.Figures;
using Core.Pause;
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
            if (_eventTable.Has<GameOverSignal>())
            {
                CreateGameOverScreen();
                _eventTable.AddEvent<PauseSignal>();
                return;
            }

            if (_eventTable.Has<ContinueForAdsSignal>())
            {
                DestroyGameOverScreen();
                _eventTable.AddEvent<UnpauseSignal>();
                _eventTable.AddEvent<FigureSpawnSignal>();
            }
        }

        private async void CreateGameOverScreen()
        {
            var handle = Addressables.InstantiateAsync("GameOverScreen");
            await handle.Task;
            _gameOverScreen = handle.Result;
            _gameOverScreen.GetComponent<GameOverMono>().OnTryAgain += StartGame;
            _gameOverScreen.GetComponent<GameOverMono>().OnAdContinue += OnAdContinueClick;
        }

        private void OnAdContinueClick()
        {
            _eventTable.AddEvent<GameOverAdsSignal>();
        }

        private void StartGame()
        {
            _world.DeactivateModule<CoreModule>();
            _world.ActivateModule<CoreModule>();
            _eventTable.AddEvent<UnpauseSignal>();
            DestroyGameOverScreen();
        }

        private void DestroyGameOverScreen()
        {
            Addressables.ReleaseInstance(_gameOverScreen);
        }
    }
}