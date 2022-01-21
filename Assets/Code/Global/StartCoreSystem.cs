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
        private EcsFilter<StartCoreComponent> _startFilter;

        public void Run()
        {
            if (_startFilter.GetEntitiesCount() == 0)
                return;

            var startData = _startFilter.Get1(0);
            _world.ActivateModule<CoreModule>();
            _playerData.AdsWasUsedInCore = false;
            _saveService.SetHasGame(true);
        }
    }
}