using System.Linq;
using EcsCore;
using Global;
using Global.Settings;
using Global.Settings.Core;
using Leopotam.Ecs;

namespace Core.Moving
{
    /// <summary>
    /// Отвечает за увеличение скорости движения фигур
    /// </summary>
    [EcsSystem(typeof(CoreModule))]
    public class MovingSpeedSystem : IEcsInitSystem, IEcsRunSystem
    {
        private PlayerData _playerData;
        private int _lastScore;
        private MovingData _movingData;
        private CoreSettings _coreSettings;
        private CoreSpeedProgression[] _speedProgression;

        public void Init()
        {
            _speedProgression = _coreSettings.fallSpeedProgression.OrderByDescending(p => p.scores).ToArray();
            UpdateSpeed();
        }

        public void Run()
        {
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

                _movingData.currentFallSpeed = progression.speed;
                break;
            }
        }
    }
}