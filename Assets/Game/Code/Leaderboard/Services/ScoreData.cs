using System.Globalization;
using UnityEngine;

namespace Global.Leaderboard.Services
{
    public struct ScoreData
    {
        public readonly long scores;
        public readonly string nickname;
        public long place;
        public string userId;

        public ScoreData(long scores, string nickname)
        {
            this.scores = scores;
            this.nickname = nickname;
            place = 1;
            userId = "Def" + Random.Range(float.MinValue, float.MaxValue).ToString(CultureInfo.InvariantCulture);
        }
    }
}