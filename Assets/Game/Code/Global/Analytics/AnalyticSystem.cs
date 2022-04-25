﻿using ByteBrewSDK;
using Core.Ads;
using Core.GameOver;
using EcsCore;
using Leopotam.Ecs;
using Utilities;

namespace Global.Analytics
{
    [EcsSystem(typeof(MainModule))]
    public class AnalyticSystem : IEcsInitSystem, IEcsRunSystem
    {
        private const string INSTALL_KEY = "analytic.install";
        private EcsFilter<AnalyticTag> _analytics;
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
            if (_eventTable.Has<GameOverSignal>())
            {
                if (_playerData.AdsWasUsedInCore)
                    ByteBrew.NewCustomEvent("round_defeat");
                else
                    ByteBrew.NewCustomEvent("round_end");
            }
            if (_playerData.CurrentScores > _playerData.MaxScores)
                ByteBrew.NewCustomEvent("new_high_score");

            if (_eventTable.Has<ContinueForAdsSignal>())
                ByteBrew.NewCustomEvent("ad_video_view", 1);
            if (_eventTable.Has<AdsFailSignal>())
                ByteBrew.NewCustomEvent("ad_video_view", 0);
        }
    }
}