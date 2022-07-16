using System.Collections.Generic;
using System.Globalization;
using EcsCore;
using Global.Analytics;
using Global.Saving;
using Leopotam.Ecs;

namespace Core.Input
{
    [EcsSystem(typeof(CoreModule))]
    public class InputProcessSystem : IEcsInitSystem, IEcsDestroySystem
    {
        private EcsWorld _world;
        private SaveService _saveService;
        private EcsOneData<SwipeData> _swipeData;

        public void Init()
        {
            EcsWorldEventsBlackboard.AddEventHandler<InputRawEvent>(OnInputEvent);
        }

        private void OnInputEvent(InputRawEvent ev)
        {
            ref var swipe = ref _swipeData.GetData();

            if (swipe.state != SwipeState.Finished || !_saveService.GetTutorCompleted())
                return;

            swipe.swipeCount++;
            swipe.state = SwipeState.Start;
            swipe.direction = ev.direction;

            if (swipe.swipeCount % 10 == 0)
            {
                RegisterSwipe10Event(swipe);
            }
        }
        private void RegisterSwipe10Event(SwipeData swipe)
        {
            var data = new Dictionary<string, string>
            {
                { "swipe_num_round", swipe.swipeCount.ToString(CultureInfo.InvariantCulture) }
            };
            _world.CreateOneFrame().Replace(AnalyticHelper.CreateEvent("swipe_n_times", data));
        }

        public void Destroy()
        {
            EcsWorldEventsBlackboard.RemoveEventHandler<InputRawEvent>(OnInputEvent);
        }
    }
}