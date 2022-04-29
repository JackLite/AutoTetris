using System.Globalization;
using TMPro;
using UnityEngine;

namespace Global.Leaderboard.View
{
    public class LeaderboardRowView : MonoBehaviour
    {
        [SerializeField]
        private RectTransform playerArrow;

        [SerializeField]
        private TextMeshProUGUI placeText;

        [SerializeField]
        private TextMeshProUGUI nicknameText;

        [SerializeField]
        private TextMeshProUGUI scoreText;

        [Header("Active player settings")]
        [SerializeField]
        private Color nicknameColor;
        
        public long Place { get; private set; }
        
        public void SetData(long place, string nickname, long scores)
        {
            Place = place;
            var culture = CultureInfo.InvariantCulture;
            var format = culture.NumberFormat;
            format.NumberGroupSeparator = " ";
            placeText.text = place.ToString(format);

            nicknameText.text = nickname;
            scoreText.text = scores.ToString(culture);
        }

        public void SetAsCurrentPlayer()
        {
            playerArrow.gameObject.SetActive(true);
            nicknameText.color = nicknameColor;
            nicknameText.fontStyle |= FontStyles.Bold;
        }
    }
}