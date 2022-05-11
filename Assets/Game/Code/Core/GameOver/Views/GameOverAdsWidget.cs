using System;
using Global.UI.Timer;
using UnityEngine;
using UnityEngine.UI;

namespace Core.GameOver.Views
{
    public class GameOverAdsWidget : MonoBehaviour
    {
        [SerializeField]
        private Button adContinueBtn;

        [field:SerializeField]
        public TimerView Timer { get; private set; }
        
        public event Action OnAdContinue;

        private void Awake()
        {
            adContinueBtn.onClick.AddListener(() => OnAdContinue?.Invoke());
        }

        public void SetActive(bool isActive)
        {
            adContinueBtn.gameObject.SetActive(isActive);
            Timer.SetActive(isActive);
        }
    }
}