using System;
using UnityEngine;
using UnityEngine.UI;

namespace Core.GameOver
{
    public class GameOverMono : MonoBehaviour
    {
        [SerializeField]
        private Button tryAgainBtn;

        [field:SerializeField]
        public GameOverScoresMono Scores { get; private set; }

        [field:SerializeField]
        public GameOverAdsWidget AdsWidget { get; private set; }

        public event Action OnTryAgain;

        private void Awake()
        {
            tryAgainBtn.onClick.AddListener(() => OnTryAgain?.Invoke());
        }

        public void SetTryAgainActive(bool isActive)
        {
            tryAgainBtn.gameObject.SetActive(isActive);
        }
    }
}