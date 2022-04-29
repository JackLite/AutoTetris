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

        private LinkedList<LeaderboardRowView> _rows = new LinkedList<LeaderboardRowView>();

        public void AddScore(long place, string nickname, long value, bool isCurrentPlayer = false)
        {
            var row = Instantiate(_leaderboardRowPrefab, transform);
            _leaderboardRowPrefab.SetData(place, nickname, value);
            if(isCurrentPlayer)
                _leaderboardRowPrefab.SetAsCurrentPlayer();
            if (_rows.Count == 0 || _rows.Last.Value.Place > place)
                _rows.AddLast(row);
            else
                _rows.AddFirst(row);
        }

        public void UpdateView()
        {
            foreach (var row in _rows)
            {
                row.transform.SetAsLastSibling();
            }
            LayoutRebuilder.MarkLayoutForRebuild(transform as RectTransform);
        }
    }
}