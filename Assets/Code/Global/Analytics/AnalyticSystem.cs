using ByteBrewSDK;
using EcsCore;
using Leopotam.Ecs;

namespace Global.Analytics
{
    [EcsSystem(typeof(MainModule))]
    public class AnalyticSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter<AnalyticTag> _analytics;
        private EcsEventTable _eventTable;
        public void Init()
        {
            #if UNITY_ANDROID || UNITY_EDITOR
                ByteBrew.InitializeByteBrew();
            #endif
        }
        public void Run()
        {
            if (!ByteBrew.IsInitilized)
                return;
            if (_eventTable.Has<StartCoreSignal>())
            {
                ByteBrew.NewCustomEvent("core_start");
            }
            foreach (var i in _analytics)
            {
                var entity = _analytics.Get1(i);
                
            }
        }
    }
}