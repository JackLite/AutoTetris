using Core.Ads;
using Core.Figures;
using Core.GameOver.Components;
using Core.Pause.Signals;
using EcsCore;
using Global;
using Global.Saving;
using Global.Settings;
using Global.Settings.Core;
using Leopotam.Ecs;

namespace Core.GameOver.Systems
{
    [EcsSystem(typeof(GameOverModule))]
    public class GameOverSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsEventTable _eventTable;
        private StartCoreData _startCoreData;
        private PlayerData _playerData;
        private SaveService _saveService;
        private GlobalSettings _settings;
        private CoreSettings _coreSettings;

        public void Init()
        {
            if (_coreSettings.aiEnable)
                return;

            _eventTable.AddEvent<PauseSignal>();
            if (!_playerData.adsWasUsedInCore)
                _saveService.SetHasGame(false);
        }

        public void Run()
        {
            if (_eventTable.Has<GameOverRestartSignal>())
            {
                RestartGame();
                return;
            }

            if (_eventTable.Has<ContinueForAdsSignal>())
            {
                _eventTable.AddEvent<UnpauseSignal>();
                _eventTable.AddEvent<FigureSpawnSignal>();
                _world.DeactivateModule<GameOverModule>();
            }
        }

        private void RestartGame()
        {
            _eventTable.AddEvent<StartCoreSignal>();
            _eventTable.AddEvent<RestartCoreSignal>();
            _world.DeactivateModule<GameOverModule>();
            _world.DeactivateModule<CoreModule>();
            _startCoreData.isContinue = false;
        }
    }
}