using System.Globalization;
using TMPro;
using UnityEngine;

namespace Core.GameOver
{
    public class GameOverScoresMono : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI scoresText;

        [SerializeField]
        private GameObject newMaxGO;

        [SerializeField]
        private TextMeshProUGUI percentFromMaxText;

        public void SetScores(int scores)
        {
            scoresText.text = $"Scores: {scores.ToString()}";
        }

        public void SetNewMaxState()
        {
            newMaxGO.SetActive(true);
            percentFromMaxText.gameObject.SetActive(false);
        }

        public void SetPercentState(float percent)
        {
            newMaxGO.SetActive(false);
            var percentText = percent.ToString("F", CultureInfo.InvariantCulture);
            percentFromMaxText.text = $"{percentText}% from your maximum. Try again!";
            percentFromMaxText.gameObject.SetActive(true);
        }
    }
}