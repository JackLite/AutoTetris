using Core.GameOver;
using EcsCore;
using Global;
using Leopotam.Ecs;
using Utilities;

namespace Core.Scores
{
    [EcsSystem(typeof(CoreModule))]
    public class ScoreSystem : IEcsInitSystem, IEcsRunSystem
    {
        private MainScreenMono _mainScreenMono;
        private PlayerData _playerData;
        private EcsEventTable _eventTable;
        private int _lastScores = -1;
        private const string SAVE_SCORES_KEY = "player.scores";

        public void Init()
        {
            _playerData.CurrentScores = 0;
            _lastScores = -1;
        }

        public void Run()
        {
            
            if (_lastScores == _playerData.CurrentScores)
                return;

            _mainScreenMono.ScoreView.UpdateScores(_playerData.CurrentScores);
            _lastScores = _playerData.CurrentScores;
            SaveUtility.SaveInt(SAVE_SCORES_KEY, _lastScores);
        }
    }
}