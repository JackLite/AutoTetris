﻿using Core.GameOver.Components;
using EcsCore;
using Global.Saving;
using UnityEngine;
using Leopotam.Ecs;

#if UNITY_ANDROID || UNITY_EDITOR
using GooglePlayGames;
#endif

namespace Global.Scores
{
    [EcsSystem(typeof(MainModule))]
    public class SaveScoreSystem : IEcsInitSystem, IEcsRunSystem
    {
        private PlayerData _playerData;
        private EcsEventTable _eventTable;
        private SaveService _saveService;

        public void Init()
        {
            _playerData.maxScores = _saveService.LoadMaxScores();
        }

        public void Run()
        {
            if (!_eventTable.Has<GameOverCoreSignal>())
                return;

            Debug.Log("[Scores] Send scores in board.");
            #if UNITY_ANDROID || UNITY_EDITOR
            Debug.Log("[Scores] Send scores in board 2.");
            PlayGamesPlatform.Instance.ReportScore(_playerData.currentScores, GPGSIds.leaderboard_main, 
                b =>
                {
                    Debug.Log("[Scores] Add scores in board. Success: " + b);
                });
            #endif
            _playerData.maxScoresAchieved = _playerData.currentScores > _playerData.maxScores;

            if (_playerData.currentScores <= _playerData.maxScores)
                return;

            _playerData.maxScores = _playerData.currentScores;
            _saveService.SaveMaxScores(_playerData.maxScores);
        }
    }
}