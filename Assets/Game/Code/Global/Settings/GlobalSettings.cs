using System;
using Global.Settings.Audio;
using Global.Settings.Localization;
using UnityEngine;

namespace Global.Settings
{
    [Serializable]
    public class GlobalSettings
    {
        public LocalizationSettings localization;
        public AudioMap[] audioMap;
        public TextAsset fakeScores;
    }
}