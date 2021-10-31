using Core.Moving;
using EcsCore;
using Leopotam.Ecs;

namespace Core.Figures
{
    [EcsSystem(typeof(CoreSetup))]
    public class FigureSystem : IEcsRunSystem
    {
        private EcsFilter<FigureComponent, PositionComponent, PositionChangeSignal> _filter;
        public void Run()
        {
            foreach (var i in _filter)
            {
                ref var figure = ref _filter.Get1(i);
                ref var pos = ref _filter.Get2(i);
                figure.Mono.SetGridPosition(pos.Row, pos.Column);
                _filter.GetEntity(i).Del<PositionChangeSignal>();
            }
        }
    }
}