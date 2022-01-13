using Core;
using EcsCore;
using Leopotam.Ecs;

namespace Global
{
    [EcsSystem(typeof(MainModule))]
    public class StartCoreSystem : IEcsRunSystem
    {
        private EcsWorld _world;
        private PlayerData _playerData;
        private EcsEventTable _eventTable;

        public void Run()
        {
            if (!_eventTable.Has<StartCoreSignal>())
                return;

            _world.ActivateModule<CoreModule>();
            _playerData.AdsWasUsedInCore = false;
        }
    }
}