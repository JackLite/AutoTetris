using Core.Figures;
using Core.Grid;
using Core.Moving;
using Core.Path;
using EcsCore;
using Leopotam.Ecs;

namespace Core.AI
{
    [EcsSystem(typeof(CoreModule))]
    public class AiCheckReachableSystem : IEcsRunSystem
    {
        private EcsFilter<MoveFigureSignal> _filter;
        private EcsFilter<AiDecision> _decisionsFilter;
        private EcsFilter<Figure>.Exclude<FigureMoveChosen> _figureFilter;
        private GridData _grid;
        private MainScreenMono _mainScreen;

        public void Run()
        {
            if (_filter.GetEntitiesCount() == 0)
                return;

            if (_figureFilter.GetEntitiesCount() == 0)
            {
                foreach (var mi in _filter)
                    _filter.GetEntity(mi).Replace(new EcsOneFrame());
                return;
            }
            var figure = _figureFilter.Get1(0);
            foreach (var mi in _filter)
            {
                _filter.GetEntity(mi).Replace(new EcsOneFrame());
                foreach (var i in _decisionsFilter)
                {
                    ref var decision = ref _decisionsFilter.Get1(i);
                    figure.rotation = decision.Rotation;
                    var path = FigurePathfinder.FindPath(figure.Position,
                        decision.Position,
                        _grid.FillMatrix,
                        figure);
                    if (path.Count == 0)
                    {
                        _decisionsFilter.GetEntity(i).Replace(new EcsOneFrame());
                        _mainScreen.ShadowCellsController.Hide(decision.Direction);
                    }
                }
            }
        }
    }
}