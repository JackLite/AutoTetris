using UnityEngine;

namespace Global.Settings.Core
{
    [CreateAssetMenu(order = 10, fileName = "CoreSettings", menuName = "Settings/CoreSettings")]
    public class CoreSettingsContainer : ScriptableObject
    {
        public CoreSettings coreSettings;
    }
}