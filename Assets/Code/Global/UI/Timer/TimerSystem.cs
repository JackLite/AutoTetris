using EcsCore;
using Leopotam.Ecs;
using UnityEngine;

namespace Global.UI.Timer
{
    [EcsSystem(typeof(MainModule))]
    public class TimerSystem : IEcsRunSystem
    {
        private EcsFilter<TimerComponent> _timers;
        public void Run()
        {
            foreach (var i in _timers)
            {
                ref var t = ref _timers.Get1(i);
                if (Time.unscaledTime < t.nextUpdateTime)
                    continue;

                t.currentSeconds--;

                t.view.SetTime(t.currentSeconds);
                t.nextUpdateTime += t.frequency;
                if (t.currentSeconds <= 0)
                    _timers.GetEntity(i).Del<TimerComponent>();
            }
        }
    }
}