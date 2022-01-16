using Core.Cells;
using Core.Grid;
using Core.Pause.Signals;
using EcsCore;
using Global;
using Leopotam.Ecs;

namespace Core.CoreDebug
{
    [EcsSystem(typeof(CoreModule))]
    public class DebugCoreSystem : IEcsInitSystem
    {
        private EcsEventTable _eventTable;
        private CoreState _coreState;
        private CoreConfig _coreConfig;
        
        public void Init()
        {
            if (!_coreConfig.IsDebug) return;

            _eventTable.AddEvent<PauseSignal>();
            
        }
    }
}