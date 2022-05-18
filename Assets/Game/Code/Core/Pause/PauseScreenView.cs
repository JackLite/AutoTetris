using Global.Audio;
using UnityEngine;

namespace Core.Pause
{
    public class PauseScreenView : MonoBehaviour
    {
        [field:SerializeField]
        public AudioSettingsView AudioSettingsView { get; private set; }
    }
}