using System;
using EcsCore;
using Global.Ads;
using Leopotam.Ecs;

namespace Core.Ads
{
    [EcsSystem(typeof(CoreModule))]
    public class GameOverAdsSystem : IEcsRunSystem
    {
        private EcsEventTable _eventTable;
        private AdsService _adsService;
        private readonly Action _onSuccess;

        public GameOverAdsSystem()
        {
            _onSuccess = OnRewardedVideoSuccess;
        }

        public void Run()
        {
            if (!_eventTable.IsEventExist<GameOverAdsSignal>())
                return;
            _adsService.ShowRewardedVideo(_onSuccess);
        }

        private void OnRewardedVideoSuccess()
        {
            _eventTable.AddEvent<ContinueForAdsSignal>();
        }
    }
}