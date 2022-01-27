using EcsCore;
using Global;
using Leopotam.Ecs;

namespace Core.Moving
{
    /// <summary>
    /// Отвечает за увеличение скорости движения фигур
    /// </summary>
    [EcsSystem(typeof(CoreModule))]
    public class MovingSpeedSystem : IEcsRunSystem
    {
        private PlayerData _playerData;
        private int _lastScore;
        private MovingData _movingData;

        public void Run()
        {
            if (_playerData.CurrentScores == _lastScore)
                return;

            _lastScore = _playerData.CurrentScores;
            var factor = _lastScore / 100;
            //_movingData.currentFallSpeed = _movingData.startFallSpeed * (1 + factor * .5f);
        }
    }
}