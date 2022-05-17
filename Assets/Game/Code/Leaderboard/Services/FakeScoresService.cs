using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Global.Leaderboard.Services
{
    public class FakeScoresService
    {
        public List<ScoreData> FakeScores { get; }

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
            for (var i = 0; i < temp.Length; ++i)
            {
                ref var s = ref temp[i];
                s.place = i + 1;
            }
            FakeScores = new List<ScoreData>(temp);
        }
    }
}