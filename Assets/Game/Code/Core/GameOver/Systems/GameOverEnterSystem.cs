using Core.GameOver.Components;
using EcsCore;
using Global;
using GooglePlayGames;
using Leopotam.Ecs;

namespace Core.GameOver.Systems
{
    [EcsSystem(typeof(CoreModule))]
    public class GameOverEnterSystem : IEcsRunLateSystem
    {
        private EcsEventTable _eventTable;
        private EcsWorld _world;
        public void RunLate()
        {
            if (_eventTable.Has<GameOverCoreSignal>())
            {
                _world.ActivateModule<GameOverModule>();
            }
        }
    }
}