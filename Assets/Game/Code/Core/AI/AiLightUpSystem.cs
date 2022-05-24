using System.Collections.Generic;
using Core.Cells;
using Core.Figures;
using Core.Figures.FigureAlgorithms;
using Core.Grid;
using EcsCore;
using Leopotam.Ecs;

namespace Core.AI
{
    [EcsSystem(typeof(CoreModule))]
    public class AiLightUpSystem : IEcsRunLateSystem
    {
        private EcsFilter<AiDecision> _decisionsFilter;
        private EcsFilter<Cell> _cells;
        private EcsFilter<Figure> _filter;

        private readonly LinkedList<AiDecision> _decisions = new();

        public void RunLate()
        {
            if (_filter.GetEntitiesCount() == 0)
                return;

            ref var figure = ref _filter.Get1(0);
            _decisions.Clear();
            foreach (var i in _decisionsFilter)
            {
                ref var decision = ref _decisionsFilter.Get1(i);
                _decisions.AddLast(decision);
            }
            LightUpMoves(figure);
        }

        private void LightUpMoves(Figure figure)
        {
            foreach (var i in _cells)
            {
                ref var cell = ref _cells.Get1(i);

                var isNeedLightUp = false;
                foreach (var decision in _decisions)
                {
                    figure.rotation = decision.Rotation;
                    var position = new GridPosition(decision.Row, decision.Column);
                    if (FigureAlgorithmFacade.IsFigureAtCell(figure, cell, position))
                    {
                        if (cell.lightUpDirection != decision.Direction)
                            cell.view.LightUp(figure, decision.Direction);
                        cell.isLightUp = true;
                        isNeedLightUp = true;
                        break;
                    }
                }

                if (!isNeedLightUp && cell.isLightUp)
                {
                    cell.isLightUp = false;
                    cell.view.LightDown();
                }
            }
        }
    }
}