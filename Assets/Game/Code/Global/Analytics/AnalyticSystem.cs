using System.Collections.Generic;
using System.Text;
using Core.Ads;
using Core.GameOver.Components;
using EcsCore;
using Leopotam.Ecs;
using Utilities;

namespace Global.Analytics
{
    [EcsSystem(typeof(MainModule))]
    public class AnalyticSystem : IEcsInitSystem, IEcsRunLateSystem
    {
        private const string INSTALL_KEY = "analytic.install";
        private AnalyticEventService _analyticEventService;
        private EcsFilter<AnalyticEvent> _analytics;
        private EcsEventTable _eventTable;
        private PlayerData _playerData;

        public void Init()
        {
            _analyticEventService.Init();
            if (!SaveUtility.LoadBool(INSTALL_KEY))
            {
                _analyticEventService.SendEvent("install");
                SaveUtility.SaveBool(INSTALL_KEY, true, true);
            }
            _analyticEventService.SendEvent("session_start");
        }

        public void RunLate()
        {
            CheckRoundEvents();
            CheckAdsEvents();

            foreach (var i in _analytics)
            {
                ref var a = ref _analytics.Get1(i);
                if (a.data.HasInt())
                    _analyticEventService.SendEvent(a.eventId, a.data.GetInt());
                else if (a.data.HasFloat())
                    _analyticEventService.SendEvent(a.eventId, a.data.GetFloat());
                else if (a.data.HasString())
                    _analyticEventService.SendEvent(a.eventId, a.data.GetString());
                else if (a.data.HasMap())
                    _analyticEventService.SendEvent(a.eventId, ParseMapToString(a.data.GetMap()));
                else
                    _analyticEventService.SendEvent(a.eventId);
            }
        }
        private void CheckAdsEvents()
        {
            if (_eventTable.Has<ContinueForAdsSignal>())
                _analyticEventService.SendEvent("ad_video_view", 1);
            if (_eventTable.Has<AdsFailSignal>())
                _analyticEventService.SendEvent("ad_video_view", 0);
        }
        private void CheckRoundEvents()
        {
            if (_eventTable.Has<StartCoreSignal>())
                _analyticEventService.SendEvent("round_start");
            if (_eventTable.Has<GameOverCoreSignal>())
            {
                if (_playerData.adsWasUsedInCore)
                    _analyticEventService.SendEvent("round_defeat");
                else
                    _analyticEventService.SendEvent("round_end");
            }
        }

        private static string ParseMapToString(IReadOnlyDictionary<string, string> map)
        {
            var sb = new StringBuilder(map.Count * 20);

            foreach (var p in map)
            {
                sb.Append(p.Key);
                sb.Append("=");
                sb.Append(p.Value);
                sb.Append(";");
            }
            return sb.ToString();
        }
    }
}