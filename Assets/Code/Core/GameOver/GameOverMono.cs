using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.GameOver
{
    public class GameOverMono : MonoBehaviour
    {
        [SerializeField]
        private Button tryAgainBtn;

        [SerializeField]
        private Button adContinueBtn;

        [field:SerializeField]
        public GameOverScoresMono Scores { get; private set; }

        public event Action OnTryAgain;
        public event Action OnAdContinue;

        private void Awake()
        {
            tryAgainBtn.onClick.AddListener(() => OnTryAgain?.Invoke());
            adContinueBtn.onClick.AddListener(() => OnAdContinue?.Invoke());
        }

        public void SetAdsBtnActive(bool isActive)
        {
            adContinueBtn.gameObject.SetActive(isActive);
        }
    }
}