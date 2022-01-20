using Core.GameOver;
using EcsCore;
using Leopotam.Ecs;
using Utilities;

namespace Global.Scores
{
    [EcsSystem(typeof(MainModule))]
    public class SaveScoreSystem : IEcsInitSystem, IEcsRunSystem
    {
        private PlayerData _playerData;
        private EcsEventTable _eventTable;
        private const string SAVE_MAX_SCORES_KEY = "player.max_scores";

        public void Init()
        {
            _playerData.MaxScores = SaveUtility.GetInt(SAVE_MAX_SCORES_KEY);
        }

        public void Run()
        {
            if (!_eventTable.Has<RestartCoreSignal>())
                return;

            if (_playerData.CurrentScores <= _playerData.MaxScores)
                return;

            _playerData.MaxScores = _playerData.CurrentScores;
            SaveUtility.SaveInt(SAVE_MAX_SCORES_KEY, _playerData.MaxScores);
        }
    }
}