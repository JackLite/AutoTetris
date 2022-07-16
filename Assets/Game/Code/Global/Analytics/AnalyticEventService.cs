using ByteBrewSDK;
using UnityEngine;

namespace Global.Analytics
{
    public class AnalyticEventService
    {
        public bool IsInitialized => ByteBrew.IsInitilized;
        public void Init()
        {
            #if UNITY_ANDROID || UNITY_EDITOR
            ByteBrew.InitializeByteBrew();
            #endif
        }

        public void SendEvent(string id)
        {
            if (Debug.isDebugBuild)
                Debug.Log("[Analytic::SendEvent] Event id " + id);
            
            ByteBrew.NewCustomEvent(id);
        }

        public void SendEvent(string id, float val)
        {
            if (Debug.isDebugBuild)
                Debug.Log("[Analytic::SendEvent] Event id " + id + "; value: " + val);
            ByteBrew.NewCustomEvent(id, val);
        }
        
        public void SendEvent(string id, string val)
        {
            if (Debug.isDebugBuild)
                Debug.Log("[Analytic::SendEvent] Event id " + id + "; value: " + val);
            ByteBrew.NewCustomEvent(id, val);
        }
    }
}