using System;
using Global.Settings.Audio;
using Global.Settings.Localization;

namespace Global.Settings
{
    [Serializable]
    public class GlobalSettings
    {
        public LocalizationSettings localization;
        public AudioMap[] audioMap;
    }
}