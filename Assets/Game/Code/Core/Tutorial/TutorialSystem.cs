using EcsCore;
using Global;
using Global.Saving;
using Leopotam.Ecs;

namespace Core.Tutorial
{
    [EcsSystem(typeof(TutorialModule))]
    public class TutorialSystem : IEcsRunSystem
    {
        private EcsWorld _world;
        private PlayerData _playerData;
        private SaveService _saveService;
        public void Run()
        {
            if (_playerData.currentScores < 10)
                return;
            _saveService.SaveTutorCompleted();
            _world.DeactivateModule<TutorialModule>();
        }
    }
}