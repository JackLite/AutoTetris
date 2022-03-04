using UnityEngine;

namespace Global.Settings
{
    [CreateAssetMenu(fileName = "GlobalSettings", menuName = "Settings/GlobalSettings", order = 10)]
    public class GlobalSettingsContainer : ScriptableObject
    {
        public GlobalSettings globalSettings;
        public int test;
    }
}