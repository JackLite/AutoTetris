using System;
using Global.Audio;
using UnityEngine;

namespace Global.Settings.Audio
{
    [Serializable]
    public struct AudioMap
    {
        public AudioEnum type;
        public AudioClip clip;
    }
}