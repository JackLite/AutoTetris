using EcsCore;
using Global.Saving;
using Leopotam.Ecs;

namespace Core.Tutorial
{
    [EcsSystem(typeof(CoreModule))]
    public class TutorialCoreEnter : IEcsPreInitSystem
    {
        private SaveService _saveService;
        private EcsWorld _world;
        public void PreInit()
        {
            if(!_saveService.GetTutorCompleted())
                _world.ActivateModule<TutorialModule, CoreModule>();
        }
    }
}