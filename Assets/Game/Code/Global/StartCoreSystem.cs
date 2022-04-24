using Core;
using EcsCore;
using Global.Saving;
using Leopotam.Ecs;

namespace Global
{
    [EcsSystem(typeof(MainModule))]
    public class StartCoreSystem : IEcsRunSystem
    {
        private EcsWorld _world;
        private PlayerData _playerData;
        private EcsEventTable _eventTable;
        private SaveService _saveService;

        public void Run()
        {
            if (!_eventTable.Has<StartCoreSignal>())
                return;

            _world.ActivateModule<CoreModule>();
            _playerData.AdsWasUsedInCore = false;
            _saveService.SetHasGame(true);
        }
    }
}