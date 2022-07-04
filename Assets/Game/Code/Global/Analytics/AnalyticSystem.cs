using ByteBrewSDK;
using Core.Ads;
using Core.GameOver;
using Core.GameOver.Components;
using EcsCore;
using Leopotam.Ecs;
using Utilities;

namespace Global.Analytics
{
    [EcsSystem(typeof(MainModule))]
    public class AnalyticSystem : IEcsInitSystem, IEcsRunSystem
    {
        private const string INSTALL_KEY = "analytic.install";
        private EcsFilter<AnalyticEvent> _analytics;
        private EcsEventTable _eventTable;
        private PlayerData _playerData;

        public void Init()
        {
            #if UNITY_ANDROID || UNITY_EDITOR
            ByteBrew.InitializeByteBrew();
            #endif
            if (!SaveUtility.LoadBool(INSTALL_KEY))
            {
                ByteBrew.NewCustomEvent("install");
                SaveUtility.SaveBool(INSTALL_KEY, true, true);
            }
            ByteBrew.NewCustomEvent("session_start");
        }

        public void Run()
        {
            if (!ByteBrew.IsInitilized)
                return;
            if (_eventTable.Has<StartCoreSignal>())
                ByteBrew.NewCustomEvent("round_start");
            if (_eventTable.Has<GameOverCoreSignal>())
            {
                if (_playerData.adsWasUsedInCore)
                    ByteBrew.NewCustomEvent("round_defeat");
                else
                    ByteBrew.NewCustomEvent("round_end");
            }
            if (_playerData.currentScores > _playerData.maxScores)
                ByteBrew.NewCustomEvent("new_high_score");

            if (_eventTable.Has<ContinueForAdsSignal>())
                ByteBrew.NewCustomEvent("ad_video_view", 1);
            if (_eventTable.Has<AdsFailSignal>())
                ByteBrew.NewCustomEvent("ad_video_view", 0);

            foreach (var i in _analytics)
            {
                ref var a = ref _analytics.Get1(i);
                ByteBrew.NewCustomEvent(a.eventId, a.data);
            }
        }
    }
}