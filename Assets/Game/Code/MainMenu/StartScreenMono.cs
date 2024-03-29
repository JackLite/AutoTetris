﻿using Global.Audio;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public class StartScreenMono : MonoBehaviour
    {
        [field:SerializeField]
        public StartGameButton StartGameButton { get; private set; }

        [field:SerializeField]
        public AudioSettingsView AudioSettingsView { get; private set; }
        
        [field:SerializeField]
        public Button StartDebugButton { get; private set; }
    }
}