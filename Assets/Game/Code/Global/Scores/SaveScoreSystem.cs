using Core.GameOver;
using EcsCore;
using Global.Saving;
using Leopotam.Ecs;

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
            _playerData.MaxScores = _saveService.LoadMaxScores();
        }

        public void Run()
        {
            if (!_eventTable.Has<RestartCoreSignal>())
                return;

            if (_playerData.CurrentScores <= _playerData.MaxScores)
                return;

            _playerData.MaxScores = _playerData.CurrentScores;
            _saveService.SaveMaxScores(_playerData.MaxScores);
        }
    }
}