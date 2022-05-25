using EcsCore;
using Global.Saving;
using Global.Settings;
using Leopotam.Ecs;
using UnityEngine;

namespace Global.Audio
{
    [EcsSystem(typeof(MainModule))]
    public class AudioSystem : IEcsInitSystem, IEcsRunSystem, IEcsRunLateSystem, IEcsDestroySystem
    {
        private EcsFilter<AudioComponent> _sounds;
        private EcsFilter<AudioChangeEvent> _audioChangeFilter;
        private AudioSourcePool _pool;
        private AudioService _audioService;
        private GlobalSettings _settings;
        private SaveService _saveService;

        public void Init()
        {
            _audioService.SetMusicState(_saveService.GetMusicState());
            _audioService.SetSoundState(_saveService.GetSoundState());
            InitMusic();
        }
        private void InitMusic()
        {
            var s = _pool.GetSource();
            s.clip = _settings.music;
            s.loop = true;
            s.outputAudioMixerGroup = _settings.musicGroup;
            s.Play();
        }

        public void Run()
        {
            foreach (var i in _audioChangeFilter)
            {
                ref var changeSignal = ref _audioChangeFilter.Get1(i);
                if (changeSignal.isMusic)
                {
                    _audioService.SetMusicState(changeSignal.isActive);
                    _saveService.SaveMusicState(changeSignal.isActive);
                }
                else
                {
                    _audioService.SetSoundState(changeSignal.isActive);
                    _saveService.SaveSoundState(changeSignal.isActive);
                }
                _audioChangeFilter.GetEntity(i).Replace(new EcsOneFrame());
            }
            if (_audioChangeFilter.GetEntitiesCount() > 0)
                _saveService.Flush();
        }

        public void RunLate()
        {
            foreach (var i in _sounds)
            {
                if (!_saveService.GetSoundState())
                    return;
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