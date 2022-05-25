using System;
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
        public AudioClip music;
        public AudioMixerGroup musicGroup;
        public float soundDb;
        public float musicDb;
        public TextAsset fakeScores;
    }
}