﻿using System.Globalization;
using TMPro;
using UnityEngine;

namespace Core.Scores
{
    public class ScoreView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI scoresText;

        public void UpdateScores(long scores)
        {
            scoresText.text = scores.ToString("D10", CultureInfo.InvariantCulture);
        }
    }
}