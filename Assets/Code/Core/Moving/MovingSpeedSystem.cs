using System.Linq;
using Core.Ads;
using EcsCore;
using Global;
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
        private int _lastScore;
        private CoreSettings _coreSettings;
        private CoreSpeedProgression[] _speedProgression;
        private EcsEventTable _eventTable;
        private MovingData _movingData;
        private PlayerData _playerData;

        public void Init()
        {
            _speedProgression = _coreSettings.fallSpeedProgression.OrderByDescending(p => p.scores).ToArray();
            UpdateSpeed();
        }

        public void Run()
        {
            if (_eventTable.Has<ContinueForAdsSignal>())
                UpdateSpeed();
            if (_playerData.CurrentScores == _lastScore)
                return;

            _lastScore = _playerData.CurrentScores;
            UpdateSpeed();
        }

        private void UpdateSpeed()
        {
            foreach (var progression in _speedProgression)
            {
                if (progression.scores > _lastScore)
                    continue;

                _movingData.currentFallSpeed = progression.speed * _movingData.factor;
                break;
            }
            if (_movingData.factor < 1f)
            {
                _movingData.factor += (1f - _coreSettings.adsSlowFactor) / _coreSettings.adsRestoreSpeedTurns;
                _movingData.factor = Mathf.Clamp01(_movingData.factor);
            }
        }
    }
}