using Global.Settings;
using UnityEngine.Audio;

namespace Global.Audio
{
    public class AudioService
    {
        private readonly AudioMixer _mixer;
        private readonly GlobalSettings _settings;

        public AudioService(AudioMixer mixer, GlobalSettings settings)
        {
            _mixer = mixer;
            _settings = settings;
        }

        public void SetMusicState(bool isActive)
        {
            _mixer.SetFloat("MusicVolume", isActive ? _settings.musicDb : -80);
        }

        public void SetSoundState(bool isActive)
        {
            _mixer.SetFloat("SoundsVolume", isActive ? _settings.soundDb : -80);
        }
    }
}