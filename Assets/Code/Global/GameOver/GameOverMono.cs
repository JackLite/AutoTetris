using System;
using UnityEngine;
using UnityEngine.UI;

namespace Global.GameOver
{
    public class GameOverMono : MonoBehaviour
    {
        [SerializeField]
        private Button tryAgainBtn;

        public event Action OnTryAgain;
        
        private void Awake()
        {
            tryAgainBtn.onClick.AddListener(() => OnTryAgain?.Invoke());
        }
    }
}