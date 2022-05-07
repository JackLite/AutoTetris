using Core.Ads;
using Core.GameOver.Components;
using Core.GameOver.Views;
using EcsCore;
using Global;
using Global.Audio;
using Global.Leaderboard.Components;
using Global.Settings;
using Global.Settings.Core;
using Global.UI.Timer;
using Leopotam.Ecs;
using UnityEngine;

namespace Core.GameOver.Systems
{
    [EcsSystem(typeof(GameOverModule))]
    public class GameOverViewSystem : IEcsInitSystem, IEcsRunLateSystem
    {
        private GameOverMono _gameOverMono;
        private PlayerData _playerData;
        private EcsEventTable _eventTable;
        private EcsWorld _world;
        private GlobalSettings _settings;
        private CoreSettings _coreSettings;
        private EcsFilter<GameOverTimerTag>.Exclude<TimerComponent> _adsTimerFinishFilter;
        private EcsFilter<GameOverTimerTag> _adsTimerFilter;
        private EcsFilter<LeaderboardScore> _leaderboardScoresFilter;

        public void Init()
        {
            if (_coreSettings.aiEnable)
                return;
            InitGameOverScreen();
            _world.CreateOneFrame().Replace(AudioHelper.Create(_settings, AudioEnum.GameOver));
        }

        public void RunLate()
        {
            if (_coreSettings.aiEnable)
                return;

            if (_adsTimerFinishFilter.GetEntitiesCount() > 0)
            {
                _adsTimerFinishFilter.GetEntity(0).Destroy();
                _gameOverMono.AdsWidget.SetActive(false);
                _gameOverMono.SetTryAgainActive(true);
                InitLeaderboard();
            }
        }

        private void InitGameOverScreen()
        {
            _gameOverMono.OnTryAgain += RestartGame;
            _gameOverMono.SetTryAgainActive(_playerData.AdsWasUsedInCore);
            InitAdsWidget();

            if (_playerData.AdsWasUsedInCore)
            {
                InitLeaderboard();
            }
        }
        private void InitLeaderboard()
        {
            /*_gameOverMono.LeaderboardView.ShowScores(true);
            foreach (var i in _leaderboardScoresFilter)
            {
                ref var score = ref _leaderboardScoresFilter.Get1(i);
                _gameOverMono.LeaderboardView.AddScore(score.place, score.nickname, score.scores);
                _leaderboardScoresFilter.GetEntity(i).Destroy();
            }
            _gameOverMono.LeaderboardView.UpdateView();*/
        }

        private void InitAdsWidget()
        {
            _gameOverMono.AdsWidget.SetActive(!_playerData.AdsWasUsedInCore);
            if (_playerData.AdsWasUsedInCore)
                return;

            _gameOverMono.AdsWidget.OnAdContinue += OnAdContinueClick;

            var timerComponent = new TimerComponent(1)
            {
                view = _gameOverMono.AdsWidget.Timer,
                currentSeconds = 5
            };
            timerComponent.nextUpdateTime = Time.unscaledTime + timerComponent.frequency;
            _world.NewEntity().Replace(timerComponent).Replace(new GameOverTimerTag());
        }

        private void OnAdContinueClick()
        {
            _world.CreateOneFrame().Replace(AudioHelper.Create(_settings, AudioEnum.GUIButton));
            _eventTable.AddEvent<GameOverAdsSignal>();
            if (_adsTimerFilter.GetEntitiesCount() > 0)
                _adsTimerFilter.GetEntity(0).Destroy();
        }

        private void RestartGame()
        {
            _world.CreateOneFrame().Replace(AudioHelper.Create(_settings, AudioEnum.GUIButton));
            _eventTable.AddEvent<GameOverRestartSignal>();
        }
    }
}