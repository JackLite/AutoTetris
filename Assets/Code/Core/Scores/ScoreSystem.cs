using EcsCore;
using Global;
using Leopotam.Ecs;
using Utilities;

namespace Core.Scores
{
    [EcsSystem(typeof(CoreModule))]
    public class ScoreSystem : IEcsInitSystem, IEcsRunSystem
    {
        private MainScreenMono mainScreenMono;
        private PlayerData playerData;
        private int _lastScores = -1;

        public void Init()
        {
            playerData.Scores = 0;
            _lastScores = -1;
        }

        public void Run()
        {
            if (_lastScores == playerData.Scores)
                return;

            mainScreenMono.ScoreView.UpdateScores(playerData.Scores);
            _lastScores = playerData.Scores;
            SaveUtility.SaveInt("player.scores", _lastScores);
        }
    }
}