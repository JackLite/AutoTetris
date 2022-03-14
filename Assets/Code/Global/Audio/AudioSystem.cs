using EcsCore;
using Leopotam.Ecs;
using UnityEngine;

namespace Global.Audio
{
    [EcsSystem(typeof(MainModule))]
    public class AudioSystem : IEcsInitSystem, IEcsRunLateSystem, IEcsDestroySystem
    {
        private EcsFilter<AudioComponent> _sounds;
        private AudioSourcePool _pool;

        public void Init()
        {
            _pool = new AudioSourcePool(4, new GameObject("AudioSourcePool").transform);
        }

        public void RunLate()
        {
            foreach (var i in _sounds)
            {
                ref var sound = ref _sounds.Get1(i);
                var source = _pool.GetSource();
                source.clip = sound.clip;
                source.Play();
            }
            _pool.Check();
        }

        public void Destroy()
        {
            _pool.Dispose();
        }
    }
}