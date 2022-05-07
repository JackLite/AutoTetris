using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Global.Leaderboard.View
{
    [RequireComponent(typeof(RectTransform))]
    public class LeaderboardView : MonoBehaviour
    {
        [SerializeField]
        private LeaderboardRowView _leaderboardRowPrefab;

        [SerializeField]
        private GameObject _leaderboardLoading;

        private readonly SortedList<long, LeaderboardRowView> _rows = new SortedList<long, LeaderboardRowView>();

        public void AddScore(long place, string nickname, long value, bool isCurrentPlayer = false)
        {
            if (_rows.ContainsKey(place))
            {
                Debug.LogError("There is place " + place);
                return;
            }
            var row = Instantiate(_leaderboardRowPrefab, transform);
            row.SetData(place, nickname, value);
            if(isCurrentPlayer)
                row.SetAsCurrentPlayer();
            _rows.Add(place, row);
        }

        public void UpdateView()
        {
            foreach (var row in _rows)
            {
                row.Value.transform.SetAsLastSibling();
            }
            LayoutRebuilder.MarkLayoutForRebuild(transform as RectTransform);
        }

        public void ShowScores(bool isActive)
        {
            gameObject.SetActive(isActive);
            _leaderboardLoading.SetActive(false);
            UpdateView();
        }
    }
}