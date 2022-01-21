using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public class StartScreenMono : MonoBehaviour
    {
        [field:SerializeField]
        public StartGameButton StartGameButton { get; private set; }
        
        [field:SerializeField]
        public Button ContinueGameButton { get; private set; }
        
        [field:SerializeField]
        public Button StartDebugButton { get; private set; }
    }
}