using System.Collections.Generic;
using System.Globalization;
using Core.Ads;
using EcsCore;
using Global;
using Global.Analytics;
using Global.Saving;
using Global.Settings.Core;
using Leopotam.Ecs;
using UnityEngine;

namespace Core.Moving
{
    /// <summary>
    /// Отвечает за увеличение скорости движения фигур
    /// </summary>
    [EcsSystem(typeof(CoreModule))]
    public class MovingSpeedSystem : IEcsInitSystem, IEcsRunSystem
    {
        private long _lastScore;
        private CoreSettings _coreSettings;
        private CoreProgressionService _coreProgressionService;
        private EcsEventTable _eventTable;
        private EcsWorld _world;
        private MovingData _movingData;
        private PlayerData _playerData;
        private SaveService _saveService;

        public void Init()
        {
            if (_saveService.HasGame())
                _movingData.factor = _saveService.GetFallSpeedFactor();
            UpdateSpeed();
        }

        public void Run()
        {
            if (_eventTable.Has<ContinueForAdsSignal>())
                UpdateSpeed();
            if (_playerData.currentScores == _lastScore)
                return;

            _lastScore = _playerData.currentScores;
            var currentDifficult = _coreProgressionService.GetDifficult(_lastScore);
            if (currentDifficult.scores > _movingData.currentDifficult.scores)
            {
                var level = _coreProgressionService.GetLevel(_lastScore);
                SendNewDifficultEvent(level);
            }
            UpdateSpeed(currentDifficult);
        }
        private void SendNewDifficultEvent(int level)
        {
            var data = new Dictionary<string, string>
            {
                { "difficulty", level.ToString(CultureInfo.InvariantCulture) },
                { "is_after_continue", _playerData.adsWasUsedInCore.ToString(CultureInfo.InvariantCulture) }
            };
            _world.CreateOneFrame().Replace(AnalyticHelper.CreateEvent("difficulty_change", data));
        }

        private void UpdateSpeed()
        {
            var currentDifficult = _coreProgressionService.GetDifficult(_lastScore);
            UpdateSpeed(currentDifficult);
        }
        private void UpdateSpeed(CoreSpeedProgression currentDifficult)
        {
            _movingData.currentDifficult = currentDifficult;
            _movingData.currentFallSpeed = _movingData.currentDifficult.speed * _movingData.factor;
            if (_movingData.factor < 1f)
            {
                _movingData.factor += (1f - _coreSettings.adsSlowFactor) / _coreSettings.adsRestoreSpeedTurns;
                _movingData.factor = Mathf.Clamp01(_movingData.factor);
            }
            _saveService.SaveFallSpeedFactor(_movingData.factor);
        }
    }
}