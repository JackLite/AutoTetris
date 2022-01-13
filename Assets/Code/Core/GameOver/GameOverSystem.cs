using Core.Ads;
using Core.Figures;
using Core.Pause;
using EcsCore;
using Global;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Core.GameOver
{
    [EcsSystem(typeof(CoreModule))]
    public class GameOverSystem : IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsEventTable _eventTable;
        private GameObject _gameOverScreen;
        private GameOverMono _gameOverMono;
        private PlayerData _playerData;

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
            _gameOverMono = _gameOverScreen.GetComponent<GameOverMono>();
            _gameOverMono.OnTryAgain += RestartGame;
            _gameOverMono.OnAdContinue += OnAdContinueClick;
            _gameOverMono.SetAdsBtnActive(!_playerData.AdsWasUsedInCore);
        }

        private void OnAdContinueClick()
        {
            _eventTable.AddEvent<GameOverAdsSignal>();
        }

        private void RestartGame()
        {
            _eventTable.AddEvent<StartCoreSignal>();
            _world.DeactivateModule<CoreModule>();
            DestroyGameOverScreen();
        }

        private void DestroyGameOverScreen()
        {
            Addressables.ReleaseInstance(_gameOverScreen);
        }
    }
}