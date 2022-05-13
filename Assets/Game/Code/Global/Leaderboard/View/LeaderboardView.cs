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

        [SerializeField]
        private Transform _rowsContainer;

        [SerializeField]
        private float _startY = 480;

        [SerializeField]
        private float _size = 70;

        private readonly SortedList<long, LeaderboardRowView> _rows = new();

        public void AddScore(long place, string nickname, long value, bool isCurrentPlayer = false)
        {
            if (_rows.ContainsKey(place))
            {
                Debug.LogError("There is place " + place);
                return;
            }
            var row = Instantiate(_leaderboardRowPrefab, _rowsContainer);
            row.SetData(place, nickname, value);
            if (isCurrentPlayer)
                row.SetAsCurrentPlayer();
            _rows.Add(place, row);
        }

        public void UpdateView()
        {
            var i = 0;
            foreach (var row in _rows)
            {
                row.Value.SetY(_startY - _size * i);
                ++i;
            }
        }

        public void ShowScores(bool isActive)
        {
            _rowsContainer.gameObject.SetActive(isActive);
            _leaderboardLoading.SetActive(false);
            UpdateView();
        }
    }
}