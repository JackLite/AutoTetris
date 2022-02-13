using Core.Ads;
using Core.Figures;
using Core.Pause.Signals;
using EcsCore;
using Global;
using Global.Saving;
using Global.UI.Timer;
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
        private EcsFilter<GameOverTimerTag>.Exclude<TimerComponent> _adsTimerFilter;
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

            if (_adsTimerFilter.GetEntitiesCount() > 0)
            {
                _adsTimerFilter.GetEntity(0).Destroy();
                _gameOverMono.AdsWidget.SetActive(false);
                _gameOverMono.SetTryAgainActive(true);
            }
        }

        private async void CreateGameOverScreen()
        {
            var handle = Addressables.InstantiateAsync("GameOverScreen");
            await handle.Task;
            _gameOverScreen = handle.Result;
            _gameOverMono = _gameOverScreen.GetComponent<GameOverMono>();
            _gameOverMono.OnTryAgain += RestartGame;
            _gameOverMono.SetTryAgainActive(_playerData.AdsWasUsedInCore);
            InitAdsWidget();

            _gameOverMono.Scores.SetScores(_playerData.CurrentScores);
            if (_playerData.MaxScores > _playerData.CurrentScores)
            {
                var percent = (float) _playerData.CurrentScores / _playerData.MaxScores;
                _gameOverMono.Scores.SetPercentState(percent * 100);
            }
            else
                _gameOverMono.Scores.SetNewMaxState();
        }
        private void InitAdsWidget()
        {
            _gameOverMono.AdsWidget.OnAdContinue += OnAdContinueClick;

            if (!_playerData.AdsWasUsedInCore)
            {
                var timerComponent = new TimerComponent(1)
                {
                    view = _gameOverMono.AdsWidget.Timer,
                    currentSeconds = 5
                };
                timerComponent.nextUpdateTime = Time.unscaledTime + timerComponent.frequency;
                _world.NewEntity().Replace(timerComponent).Replace(new GameOverTimerTag());
            }

            _gameOverMono.AdsWidget.SetActive(!_playerData.AdsWasUsedInCore);
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