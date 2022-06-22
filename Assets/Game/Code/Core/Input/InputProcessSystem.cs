using EcsCore;
using Global.Saving;
using Leopotam.Ecs;

namespace Core.Input
{
    [EcsSystem(typeof(CoreModule))]
    public class InputProcessSystem : IEcsInitSystem
    {
        private EcsWorld _world;
        private SaveService _saveService;
        
        public void Init()
        {
            EcsWorldEventsBlackboard.AddEventHandler<InputRawEvent>(OnInputEvent);
        }

        private void OnInputEvent(InputRawEvent ev)
        {
            if (_saveService.GetTutorCompleted())
                _world.NewEntity().Replace(new SwipeInput { direction = ev.direction });
        }
    }
}