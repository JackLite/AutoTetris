using Core.AI;
using Core.Cells;
using Core.Grid;
using EcsCore;
using Leopotam.Ecs;
using UnityEngine;

namespace Core.Figures
{
    [EcsSystem(typeof(CoreSetup))]
    public class MoveFigureSystem : IEcsRunSystem
    {
        private float _fallCounter;
        private EcsFilter<FigureComponent> _filter;
        private EcsFilter<CellComponent> _cells;
        private MainScreenMono _screenMono;
        private GridData _grid;
        private EcsWorld _world;

        public void Run()
        {
            if (_filter.GetEntitiesCount() == 0)
                return;

            ref var figure = ref _filter.Get1(0);
            var entity = _filter.GetEntity(0);
            if (figure.Column > 0 && entity.Has<AiDecision>())
            {
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    ref var aiDecision = ref entity.Get<AiDecision>();
                    figure.Row = aiDecision.Row;
                    figure.Column = aiDecision.Column;
                    figure.Mono.SetGridPosition(figure.Row, figure.Column);
                    _fallCounter = 0.1f;
                    return;
                } 
            }

            _fallCounter -= Time.deltaTime;

            if (_fallCounter >= 0)
                return;

            if (figure.Row > 0 && !IsFall(_grid.FillMatrix, figure))
                figure.Row--;
            figure.Mono.SetGridPosition(figure.Row, figure.Column);

            if (IsFall(_grid.FillMatrix, figure))
            {
                _world.NewEntity().Replace(new FigureSpawnSignal());
                _world.NewEntity().Replace(new CheckLinesSignal());

                GridService.FillGrid(_grid.FillMatrix, figure);

                if (figure.Row + 1 > _grid.FillMatrix.GetLength(0))
                    _screenMono.ShowGameOver();
                
                CreateSingleFigures(in figure);
                
                figure.Mono.Delete();
                _filter.GetEntity(0).Del<FigureComponent>();
                _grid.Mono.LightDown();
            }

            _fallCounter = 0.1f;
        }

        private void CreateSingleFigures(in FigureComponent figure)
        {
            foreach (var i in _cells)
            {
                ref var cell = ref _cells.Get1(i);

                if (cell.Row == figure.Row || cell.Row == figure.Row + 1)
                {
                    if (cell.Column == figure.Column || cell.Column == figure.Column + 1)
                    {
                        cell.View.SetImageActive(true);
                    }
                }
            }
        }

        private static bool IsFall(in bool[,] fillMatrix, in FigureComponent figure)
        {
            var rows = fillMatrix.GetLength(0);

            if (figure.Row >= rows)
                return false;

            if (figure.Row == 0)
                return true;

            var isFillUnder = fillMatrix[figure.Row - 1, figure.Column];
            var isFillRightUnder = fillMatrix[figure.Row - 1, figure.Column + 1];

            return isFillUnder || isFillRightUnder;
        }
    }
}