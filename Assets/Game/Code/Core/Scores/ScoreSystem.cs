using EcsCore;
using Global;
using Global.Saving;
using Global.Settings.Core;
using Leopotam.Ecs;

namespace Core.Scores
{
    [EcsSystem(typeof(CoreModule))]
    public class ScoreSystem : IEcsInitSystem, IEcsRunSystem
    {
        private MainScreenMono _mainScreenMono;
        private PlayerData _playerData;
        private CoreSettings _coreSettings;
        private EcsEventTable _eventTable;
        private SaveService _saveService;
        private StartCoreData startCoreData;
        private long _lastScores = -1;

        public void Init()
        {
            if (startCoreData.isContinue && !_coreSettings.aiEnable)
            {
                _playerData.currentScores = _saveService.LoadScores();
                _mainScreenMono.ScoreView.UpdateScores(_playerData.currentScores);
                _lastScores = _playerData.currentScores;
            }
            else
            {
                _playerData.currentScores = 0;
                _lastScores = -1;
            }
        }

        public void Run()
        {
            if (_lastScores == _playerData.currentScores)
                return;

            _mainScreenMono.ScoreView.UpdateScores(_playerData.currentScores);
            _lastScores = _playerData.currentScores;
            _saveService.SaveScores(_lastScores);
        }
    }
}