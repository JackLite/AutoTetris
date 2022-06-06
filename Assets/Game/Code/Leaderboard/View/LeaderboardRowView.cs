using System.Globalization;
using TMPro;
using UnityEngine;

namespace Global.Leaderboard.View
{
    [RequireComponent(typeof(RectTransform))]
    public class LeaderboardRowView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI placeText;

        [SerializeField]
        private TextMeshProUGUI nicknameText;

        [SerializeField]
        private TextMeshProUGUI scoreText;

        [Header("Active player settings")]
        [SerializeField]
        private Color nicknameColor;

        [SerializeField]
        private TMP_FontAsset playerScoresFont;

        private RectTransform _rt;
        public long Place { get; private set; }

        private void Awake()
        {
            _rt = GetComponent<RectTransform>();
        }

        public void SetData(long place, string nickname, long scores)
        {
            Place = place;
            var culture = CultureInfo.InvariantCulture;
            var format = new NumberFormatInfo
            {
                NumberGroupSeparator = " "
            };
            placeText.text = place.ToString(format);

            nicknameText.text = nickname;
            scoreText.text = scores.ToString(culture);
        }

        public void SetAsCurrentPlayer()
        {
            nicknameText.color = nicknameColor;
            nicknameText.fontStyle |= FontStyles.Bold;

            scoreText.color = nicknameColor;
            scoreText.font = playerScoresFont;
            scoreText.fontStyle = FontStyles.Normal;
        }
        public void SetY(float y)
        {
            if (!_rt)
                _rt = GetComponent<RectTransform>();
            _rt.anchoredPosition = new Vector2(_rt.anchoredPosition.x, y);
        }
    }
}