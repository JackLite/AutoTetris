namespace Global.Leaderboard.Services
{
    public struct ScoreData
    {
        public readonly long scores;
        public readonly string nickname;
        public long place;

        public ScoreData(long scores, string nickname)
        {
            this.scores = scores;
            this.nickname = nickname;
            place = 1;
        }
    }
}