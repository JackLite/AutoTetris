using EcsCore;
using Global;
using Global.Saving;
using Leopotam.Ecs;

namespace Core.Scores
{
    [EcsSystem(typeof(CoreModule))]
    public class ScoreSystem : IEcsInitSystem, IEcsRunSystem
    {
        private MainScreenMono _mainScreenMono;
        private PlayerData _playerData;
        private EcsEventTable _eventTable;
        private SaveService _saveService;
        private CoreConfig _coreConfig;
        private int _lastScores = -1;

        public void Init()
        {
            if (_coreConfig.isContinue)
            {
                _playerData.CurrentScores = _saveService.LoadScores();
                _mainScreenMono.ScoreView.UpdateScores(_playerData.CurrentScores);
                _lastScores = _playerData.CurrentScores;
            }
            else
            {
                _playerData.CurrentScores = 0;
                _lastScores = -1;
            }
        }

        public void Run()
        {
            if (_lastScores == _playerData.CurrentScores)
                return;

            _mainScreenMono.ScoreView.UpdateScores(_playerData.CurrentScores);
            _lastScores = _playerData.CurrentScores;
            _saveService.SaveScores(_lastScores);
        }
    }
}