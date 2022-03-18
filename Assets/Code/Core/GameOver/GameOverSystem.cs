using Core.Ads;
using Core.Figures;
using Core.Pause.Signals;
using EcsCore;
using Global;
using Global.Audio;
using Global.Saving;
using Global.Settings;
using Global.Settings.Audio;
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
        private EcsFilter<GameOverTimerTag>.Exclude<TimerComponent> _adsTimerFinishFilter;
        private EcsFilter<GameOverTimerTag> _adsTimerFilter;
        private GameObject _gameOverScreen;
        private GameOverMono _gameOverMono;
        private StartCoreData _startCoreData;
        private PlayerData _playerData;
        private SaveService _saveService;
        private GlobalSettings _settings;

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

            if (_adsTimerFinishFilter.GetEntitiesCount() > 0)
            {
                _adsTimerFinishFilter.GetEntity(0).Destroy();
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

            if (_playerData.CurrentScores > _playerData.MaxScores)
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
            _world.CreateOneFrame().Replace(AudioHelper.Create(_settings, AudioEnum.GUIButton));
            _eventTable.AddEvent<GameOverAdsSignal>();
            if(_adsTimerFilter.GetEntitiesCount() > 0)
                _adsTimerFilter.GetEntity(0).Destroy();
        }

        private void RestartGame()
        {
            _world.CreateOneFrame().Replace(AudioHelper.Create(_settings, AudioEnum.GUIButton));
            _eventTable.AddEvent<StartCoreSignal>();
            _eventTable.AddEvent<RestartCoreSignal>();
            _world.DeactivateModule<CoreModule>();
            _startCoreData.isContinue = false;
            DestroyGameOverScreen();
        }

        private void DestroyGameOverScreen()
        {
            Addressables.ReleaseInstance(_gameOverScreen);
        }
    }

}