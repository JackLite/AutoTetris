using System.Linq;
using Core.GameOver.Views;
using EcsCore;
using Global;
using Global.Leaderboard.Services;
using Global.Settings;
using Global.Settings.Core;
using GooglePlayGames;
using Leopotam.Ecs;
using UnityEngine;
using Application = UnityEngine.Device.Application;

namespace Core.GameOver.Systems
{
    [EcsSystem(typeof(GameOverModule))]
    public class GameOverLeaderboardSystem : IEcsPreInitSystem, IEcsInitSystem
    {
        private FakeScoresService _fakeScores;
        private ScoresService _scoresService;
        private EcsEventTable _eventTable;
        private EcsWorld _world;
        private CoreSettings _coreSettings;
        private GlobalSettings _globalSettings;
        private PlayerData _playerData;
        private GameOverMono _gameOverMono;
        public void PreInit()
        {
            _fakeScores = new FakeScoresService(_globalSettings.fakeScores.text);
        }

        public void Init()
        {
            if (Application.isEditor)
                ShowFake();
            else
                LoadReal();
        }

        private void LoadReal()
        {
            _scoresService.LoadScores(5,
                data =>
                {
                    Debug.Log("[Scores] Scores loaded. Count " + data.Count);

                    foreach (var s in data)
                    {
                        _gameOverMono.LeaderboardView.AddScore(s.place,
                            s.nickname,
                            s.scores,
                            s.nickname == PlayGamesPlatform.Instance.localUser.userName);
                    }
                    _gameOverMono.LeaderboardView.ShowScores(true);
                });
        }

        private void ShowFake()
        {
            var after = _fakeScores.GetScoresAfter(2, _playerData.MaxScores);
            var before = _fakeScores.GetScoresBefore(2, _playerData.MaxScores);

            foreach (var fakeScore in before)
                _gameOverMono.LeaderboardView.AddScore(fakeScore.place + 1, fakeScore.nickname, fakeScore.scores);
            foreach (var fakeScore in after)
                _gameOverMono.LeaderboardView.AddScore(fakeScore.place, fakeScore.nickname, fakeScore.scores);
            var playerPlace = 1L;
            if (after.Count > 0)
            {
                var nextPlace = after.Max(s => s.place);
                playerPlace = nextPlace + 1;
            }
            _gameOverMono.LeaderboardView.AddScore(playerPlace, "You", _playerData.MaxScores, true);
            _gameOverMono.LeaderboardView.ShowScores(true);
        }
    }
}