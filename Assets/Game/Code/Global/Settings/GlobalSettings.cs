﻿using System;
using Global.Settings.Audio;
using Global.Settings.Localization;
using UnityEngine;
using UnityEngine.Audio;

namespace Global.Settings
{
    [Serializable]
    public class GlobalSettings
    {
        public LocalizationSettings localization;
        public AudioMap[] audioMap;
        public AudioMixer mixer;
        public TextAsset fakeScores;
    }
}