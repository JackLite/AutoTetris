using System;
using Global.Leaderboard.View;
using UnityEngine;
using UnityEngine.UI;

namespace Core.GameOver.Views
{
    public class GameOverMono : MonoBehaviour
    {
        [SerializeField]
        private Button tryAgainBtn;

        [field:SerializeField]
        public LeaderboardView LeaderboardView { get; private set; }

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