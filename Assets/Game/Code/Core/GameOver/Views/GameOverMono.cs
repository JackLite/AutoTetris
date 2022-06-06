using System;
using System.Globalization;
using Global.Leaderboard.View;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.GameOver.Views
{
    public class GameOverMono : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _title;
        
        [SerializeField]
        private Button tryAgainBtn;

        [SerializeField]
        private GameObject _newTopRecord;
        
        [SerializeField]
        private GameObject _defaultLeaderboardTitle;

        [SerializeField]
        private TextMeshProUGUI _currentScores;
        
        [field:SerializeField]
        public LeaderboardView LeaderboardView { get; private set; }

        [field:SerializeField]
        public GameOverAdsWidget AdsWidget { get; private set; }

        public event Action OnTryAgain;

        private void Awake()
        {
            tryAgainBtn.onClick.AddListener(() => OnTryAgain?.Invoke());
        }

        public void UpdateTitle(bool isAdsAvailable)
        {
            _title.text = isAdsAvailable ? "Continue?" : "Game over";
        }
        
        public void SetTryAgainActive(bool isActive)
        {
            UpdateTitle(!isActive);
            tryAgainBtn.gameObject.SetActive(isActive);
        }

        public void SetNewTopRecord(bool state)
        {
            _newTopRecord.SetActive(state);
            _defaultLeaderboardTitle.SetActive(!state);
        }
        public void SetCurrentScores(long scores)
        {
            _currentScores.text = scores.ToString(CultureInfo.InvariantCulture);
        }
    }
}