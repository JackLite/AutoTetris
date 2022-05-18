using UnityEngine.Audio;

namespace Global.Audio
{
    public class AudioService
    {
        private readonly AudioMixer _mixer;
        
        public AudioService(AudioMixer mixer)
        {
            _mixer = mixer;
        }

        public void SetMusicState(bool isMusicActive)
        {
            SetAudioState(isMusicActive, "MusicVolume");
        }
        private void SetAudioState(bool isActive, string paramName)
        {
            _mixer.SetFloat(paramName, isActive ? 0 : -80);
        }

        public void SetSoundState(bool isSoundActive)
        {
            SetAudioState(isSoundActive, "SoundsVolume");
        }
    }
}