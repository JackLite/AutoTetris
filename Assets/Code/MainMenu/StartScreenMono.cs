using UnityEngine;

namespace MainMenu
{
    public class StartScreenMono : MonoBehaviour
    {
        [field:SerializeField]
        public StartGameButton StartGameButton { get; private set; }
    }
}