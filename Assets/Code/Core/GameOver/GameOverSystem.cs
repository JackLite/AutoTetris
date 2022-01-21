using Core.Ads;
using Core.Figures;
using Core.Pause;
using Core.Pause.Signals;
using EcsCore;
using Global;
using Global.Saving;
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
        private SaveService _saveService;

        public void Run()
        {
            if (_eventTable.Has<GameOverSignal>())
            {
                CreateGameOverScreen();
                _eventTable.AddEvent<PauseSignal>();
                if (!_playerData.AdsWasUsedInCore)
                    _saveService.SetHasGame(false);
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

            _gameOverMono.Scores.SetScores(_playerData.CurrentScores);
            if (_playerData.MaxScores > _playerData.CurrentScores)
            {
                var percent = (float) _playerData.CurrentScores / _playerData.MaxScores;
                _gameOverMono.Scores.SetPercentState(percent * 100);
            }
            else
                _gameOverMono.Scores.SetNewMaxState();
        }

        private void OnAdContinueClick()
        {
            _eventTable.AddEvent<GameOverAdsSignal>();
        }

        private void RestartGame()
        {
            _eventTable.AddEvent<StartCoreSignal>();
            _eventTable.AddEvent<RestartCoreSignal>();
            _world.DeactivateModule<CoreModule>();
            DestroyGameOverScreen();
        }

        private void DestroyGameOverScreen()
        {
            Addressables.ReleaseInstance(_gameOverScreen);
        }
    }

}