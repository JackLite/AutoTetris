using UnityEngine;

namespace Global
{
    public class StartScreenMono : MonoBehaviour
    {
        [field:SerializeField]
        public StartGameButton StartGameButton { get; private set; }
    }
}