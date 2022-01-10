using System;
using UnityEngine;

namespace Global.Ads
{
    public class AdsService
    {
        public void ShowRewardedVideo(Action onSuccess, Action onFailed = null)
        {
            if (Application.isEditor)
            {
                onSuccess?.Invoke();
                return;
            }
            
            //TODO: implemets ironsource video
        }
    }
}