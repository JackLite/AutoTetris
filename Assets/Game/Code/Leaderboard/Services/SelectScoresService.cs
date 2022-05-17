using System.Collections.Generic;

namespace Global.Leaderboard.Services
{
    internal class SelectScoresService
    {
        private readonly FakeScoresComparer _comparer = new FakeScoresComparer();
        private readonly FakeScoresRevertComparer _revertComparer = new FakeScoresRevertComparer();
        
        public List<ScoreData> GetScoresBefore(int before, long scores, List<ScoreData> scoresList)
        {
            var res = new List<ScoreData>(before);
            scoresList.Sort(_revertComparer);
            foreach (var score in scoresList)
            {
                if (score.scores > scores)
                    continue;
                res.Add(score);
                if (res.Count == before)
                    break;
            }

            return res;
        }
        
        public List<ScoreData> GetScoresAfter(int after, long scores, List<ScoreData> scoresList)
        {
            var res = new List<ScoreData>(after);
            scoresList.Sort(_comparer);
            foreach (var score in scoresList)
            {
                if (score.scores <= scores)
                    continue;
                res.Add(score);
                if (res.Count == after)
                    break;
            }

            return res;
        }
        public void FillTo(List<ScoreData> data, int count, List<ScoreData> fromData)
        {
            var isFromStart = false;
            var index = 0;
            var rIndex = fromData.Count - 1;
            data.Sort(_comparer);
            while (data.Count < count && index < rIndex)
            {
                if (isFromStart)
                {
                    data.Add(fromData[index]);
                    ++index;
                }
                else
                {
                    data.Add(fromData[rIndex]);
                    --rIndex;
                }
                isFromStart = !isFromStart;
            }
            data.Sort(_comparer);
        }
    }
}