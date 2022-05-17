using System;
using System.Collections.Generic;

namespace Global.Leaderboard.Services
{
    public class FakeScoresRevertComparer : IComparer<ScoreData>
    {
        public int Compare(ScoreData x, ScoreData y)
        {
            if (x.scores == y.scores)
                return string.Compare(y.nickname, x.nickname, StringComparison.OrdinalIgnoreCase);
            return y.scores.CompareTo(x.scores);
        }
    }
}