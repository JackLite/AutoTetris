using System.Linq;
using EcsCore;
using Global.Saving;
using Global.Settings;
using Global.Settings.Audio;
using Leopotam.Ecs;
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

        public static void CreateChangeMusicState(EcsWorld world, SaveService saveService)
        {
            world.NewEntity()
                 .Replace(new AudioChangeEvent { isActive = !saveService.GetMusicState(), isMusic = true });
        }

        public static void CreateChangeSoundState(EcsWorld world, SaveService saveService)
        {
            world.NewEntity().Replace(new AudioChangeEvent { isActive = !saveService.GetSoundState() });
        }
    }
}