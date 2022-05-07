using System.Collections.Generic;
using System.Linq;
using EcsCore;
using Newtonsoft.Json.Linq;
using Unity.Mathematics;

namespace Global.Leaderboard.Services
{
    public class FakeScoresService
    {
        private readonly List<ScoreData> _fakeScores;
        private readonly FakeScoresComparer _comparer = new FakeScoresComparer();
        private readonly FakeScoresRevertComparer _revertComparer = new FakeScoresRevertComparer();
        public FakeScoresService(string json)
        {
            var arr = JArray.Parse(json);
            var temp = new ScoreData[arr.Count];
            var index = 0;
            foreach (var token in arr)
            {
                temp[index] = new ScoreData(token.Value<long>("scores"), token.Value<string>("name"));
                ++index;
            }

            temp = temp.OrderByDescending(fs => fs.scores).ToArray();
            for(var i = 0; i < temp.Length; ++i)
            {
                ref var s = ref temp[i];
                s.place = i + 1;
            }
            _fakeScores = new List<ScoreData>(temp);
        }

        public List<ScoreData> GetScoresBefore(int before, long scores)
        {
            var res = new List<ScoreData>(before);
            _fakeScores.Sort(_revertComparer);
            foreach (var fakeScore in _fakeScores)
            {
                if (fakeScore.scores > scores)
                    continue;
                res.Add(fakeScore);
                if (res.Count == before)
                    break;
            }

            return res;
        }
        
        public List<ScoreData> GetScoresAfter(int after, long scores)
        {
            var res = new List<ScoreData>(after);
            _fakeScores.Sort(_comparer);
            foreach (var fakeScore in _fakeScores)
            {
                if (fakeScore.scores < scores)
                    continue;
                res.Add(fakeScore);
                if (res.Count == after)
                    break;
            }

            return res;
        }

        public List<ScoreData> GetScores(int before, int after, long scores)
        {
            var res = new List<ScoreData>(before + after + 1);
            var index = _fakeScores.FindLastIndex(fs => fs.scores > scores) - 1;
            if (index == -1)
                index = 0;
            var startFrom = math.max(0, index - before);
            var end = math.min(_fakeScores.Count - 1, index + after);
            var index2 = 0;
            foreach (var s in _fakeScores)
            {
                if (index2 >= startFrom && index2 <= end)
                    res.Add(s);
                if (index2 > end)
                    break;
                ++index2;
            }
            return res;
        }
    }
}