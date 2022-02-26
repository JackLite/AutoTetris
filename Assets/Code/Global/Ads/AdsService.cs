using System;
using UnityEngine;

namespace Global.Ads
{
    public class AdsService
    {
        private const string APP_KEY = "13759d6c1";
        private Action _onSuccess;
        public void Init()
        {
            IronSource.Agent.validateIntegration();
            IronSource.Agent.init(APP_KEY);
            IronSourceEvents.onRewardedVideoAdRewardedEvent += OnRewardedVideoReward;
        }
        public void ShowRewardedVideo(Action onSuccess, Action onFailed = null)
        {
            if (Application.isEditor)
            {
                onSuccess?.Invoke();
                return;
            }

            if (!IronSource.Agent.isRewardedVideoAvailable())
            {
                onFailed?.Invoke();
                return;
            }
            _onSuccess = onSuccess;

            IronSource.Agent.showRewardedVideo();
        }

        private void OnRewardedVideoReward(IronSourcePlacement ironSourcePlacement)
        {
            if (_onSuccess == null)
                return;
            
            _onSuccess.Invoke();
            _onSuccess = null;
        }
    }
}