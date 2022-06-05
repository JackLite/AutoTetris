using Core.Ads;
using EcsCore;
using Global;
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
            UpdateSpeed();
        }

        private void UpdateSpeed()
        {
            _movingData.currentFallSpeed = _coreProgressionService.GetSpeed(_lastScore) * _movingData.factor;
            if (_movingData.factor < 1f)
            {
                _movingData.factor += (1f - _coreSettings.adsSlowFactor) / _coreSettings.adsRestoreSpeedTurns;
                _movingData.factor = Mathf.Clamp01(_movingData.factor);
            }
            _saveService.SaveFallSpeedFactor(_movingData.factor);
        }
    }
}