using Global.Leaderboard.View;
using UnityEngine;

namespace Core.GameOver.Views
{
    public class GameOverScoresMono : MonoBehaviour
    {
        [SerializeField]
        private GameObject newMaxGO;

        [field:SerializeField]
        public LeaderboardView LeaderboardView { get; private set; }

        public void SetNewMaxState()
        {
            newMaxGO.SetActive(true);
        }
    }
}