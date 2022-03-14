using System;
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