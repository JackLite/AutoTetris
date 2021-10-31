using UnityEngine;

namespace Core
{
    public class MainScreenMono : MonoBehaviour
    {
        public RectTransform grid;

        [SerializeField]
        private GameObject gameOver;

        public void ShowGameOver()
        {
            gameOver.SetActive(true);
        }
    }
}