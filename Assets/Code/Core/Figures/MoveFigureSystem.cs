using Core.AI;
using Core.Cells;
using Core.Figures.FigureAlgorithms;
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
        private EcsFilter<Figure> _filter;
        private EcsFilter<Cell> _cells;
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
                    figure.Rotation = aiDecision.Rotation;
                    figure.Mono.SetGridPosition(figure.Row, figure.Column);
                    _fallCounter = 1f;
                    FinishMove(figure);
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
                FinishMove(figure);
            }

            _fallCounter = 1f;
        }

        private void FinishMove(Figure figure)
        {
            _world.NewEntity().Replace(new FigureSpawnSignal());
            _world.NewEntity().Replace(new CheckLinesSignal());

            FigureAlgorithmFacade.FillGrid(_grid.FillMatrix, figure);

            if (figure.Row + 1 > _grid.FillMatrix.GetLength(0))
                _screenMono.ShowGameOver();

            CreateSingleFigures(in figure);

            figure.Mono.Delete();
            _filter.GetEntity(0).Del<Figure>();
        }

        private void CreateSingleFigures(in Figure figure)
        {
            foreach (var i in _cells)
            {
                ref var cell = ref _cells.Get1(i);
                cell.View.LightDown();
                FigureAlgorithmFacade.UpdateFillCell(figure, cell);
            }
        }

        private static bool IsFall(in bool[,] fillMatrix, in Figure figure)
        {
            return FigureAlgorithmFacade.IsFall(fillMatrix, figure);
        }
    }
}