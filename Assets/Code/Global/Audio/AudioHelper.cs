using System.Linq;
using Global.Settings;
using Global.Settings.Audio;
using UnityEngine;

namespace Global.Audio
{
    public static class AudioHelper
    {
        public static AudioClip GetClip(GlobalSettings settings, AudioEnum type)
        {
            var audioMap = settings.audioMap.FirstOrDefault(am => am.type == type);
            return audioMap.clip;
        }

        public static AudioComponent Create(GlobalSettings settings, AudioEnum type)
        {
            return new AudioComponent { clip = GetClip(settings, type) };
        }
    }
}