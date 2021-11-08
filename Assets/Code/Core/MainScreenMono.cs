using Core.Input;
using UnityEngine;

namespace Core
{
    public class MainScreenMono : MonoBehaviour
    {
        public RectTransform grid;

        [SerializeField]
        private GameObject gameOver;
        
        [field:SerializeField]
        public SwipeMono SwipeMono { get; private set; }

        public void ShowGameOver()
        {
            gameOver.SetActive(true);
        }
    }
}