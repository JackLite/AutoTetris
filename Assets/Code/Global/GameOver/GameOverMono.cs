using System;
using UnityEngine;
using UnityEngine.UI;

namespace Global.GameOver
{
    public class GameOverMono : MonoBehaviour
    {
        [SerializeField]
        private Button tryAgainBtn;

        [SerializeField] 
        private Button adContinueBtn;

        public event Action OnTryAgain;
        public event Action OnAdContinue;
        
        private void Awake()
        {
            tryAgainBtn.onClick.AddListener(() => OnTryAgain?.Invoke());
            adContinueBtn.onClick.AddListener(() => OnAdContinue?.Invoke());
        }
    }
}