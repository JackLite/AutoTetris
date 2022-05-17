using System.Collections.Generic;
using System.Linq;
using Core.GameOver.Views;
using EcsCore;
using Global;
using Global.Leaderboard.Services;
using Global.Settings.Core;
using GooglePlayGames;
using Leopotam.Ecs;
using UnityEngine;
using Application = UnityEngine.Device.Application;

namespace Core.GameOver.Systems
{
    [EcsSystem(typeof(GameOverModule))]
    public class GameOverLeaderboardSystem : IEcsInitSystem
    {
        private FakeScoresService _fakeScores;
        private SelectScoresService _selectScoresService;
        private ScoresService _scoresService;
        private EcsEventTable _eventTable;
        private EcsWorld _world;
        private CoreSettings _coreSettings;
        private PlayerData _playerData;
        private GameOverMono _gameOverMono;

        public void Init()
        {
            if (Application.isEditor)
                Show(_fakeScores.FakeScores);
            else
                LoadReal();
        }

        private void LoadReal()
        {
            const int COUNT = 5;
            _scoresService.LoadScores(COUNT,
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

        private void Show(List<ScoreData> scoresList)
        {
            var after = _selectScoresService.GetScoresAfter(2, _playerData.MaxScores, scoresList);
            var before = _selectScoresService.GetScoresBefore(4 - after.Count, _playerData.MaxScores, scoresList);
            after = _selectScoresService.GetScoresAfter(4 - before.Count, _playerData.MaxScores, scoresList);

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