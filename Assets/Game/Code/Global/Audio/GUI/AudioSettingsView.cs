using UnityEngine;
using UnityEngine.UI;

namespace Global.Audio
{
    public class AudioSettingsView : MonoBehaviour
    {
        [field:SerializeField]
        public AudioButton MusicButton;

        [field:SerializeField]
        public AudioButton SoundButton;
    }
}