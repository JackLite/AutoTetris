using EcsCore;
using Global;
using Leopotam.Ecs;

namespace Core.Scores
{
    [EcsSystem(typeof(CoreModule))]
    public class ScoreSystem : IEcsRunSystem
    {
        private MainScreenMono mainScreenMono;
        private PlayerData playerData;
        private int _lastScores = -1;
        
        public void Run()
        {
            if (_lastScores == playerData.Scores)
                return;
            
            mainScreenMono.ScoreView.UpdateScores(playerData.Scores);
            _lastScores = playerData.Scores;
        }
    }
}