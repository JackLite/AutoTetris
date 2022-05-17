using System;
using System.Collections.Generic;

namespace Global.Leaderboard.Services
{
    public class FakeScoresComparer : IComparer<ScoreData>
    {
        public int Compare(ScoreData x, ScoreData y)
        {
            if (x.scores == y.scores)
                return string.Compare(x.nickname, y.nickname, StringComparison.OrdinalIgnoreCase);
            return x.scores.CompareTo(y.scores);
        }
    }
}