using System.Collections.Generic;
using System.Linq;
using Core.Cells;
using Core.Figures;
using Core.Figures.FigureAlgorithms;
using Core.Figures.FigureAlgorithms.Path;
using Core.Grid;
using EcsCore;
using Leopotam.Ecs;
using UnityEngine;

namespace Core.AI
{
    [EcsSystem(typeof(CoreModule))]
    public class AiSystem : IEcsRunSystem
    {
        private const int MOVES_COUNT = 3;
        private const float AHM = -0.510066f;
        private const float CLM = 0.760066f;
        private const float HM = -0.35663f;
        private const float BM = -0.184483f;
        private static readonly Dictionary<int, Direction> Directions = new Dictionary<int, Direction>
        {
            { 0, Direction.Left },
            { 1, Direction.Down },
            { 2, Direction.Right },
        };
        private EcsFilter<Figure> _filter;
        private EcsFilter<AiDecision> _decisionsFilter;
        private EcsFilter<Cell> _cells;
        private GridData _gridData;
        private EcsWorld _world;
        private float _timer;
        private CoreState _coreState;

        public void Run()
        {
            if (!_gridData.IsGridStable || _coreState.IsPaused)
                return;

            if (_filter.GetEntitiesCount() == 0 || _decisionsFilter.GetEntitiesCount() > 0)
            {
                _timer = .1f;
                return;
            }

            if (_timer > 0)
            {
                _timer -= Time.deltaTime;

                return;
            }
            ref var figure = ref _filter.Get1(0);

            if (figure.Row > 20)
                return;

            var aiDecisions = FindBetterMoves(_gridData.FillMatrix, ref figure).ToArray();
            foreach (var decision in aiDecisions)
            {
                _world.NewEntity().Replace(decision);
            }
            LightUpMoves(figure, aiDecisions);
        }

        private void LightUpMoves(Figure figure, in IEnumerable<AiDecision> aiDecisions)
        {
            foreach (var decision in aiDecisions)
            {
                if (decision.Direction == Direction.None)
                    continue;
                figure.Rotation = decision.Rotation;
                var position = new GridPosition(decision.Row, decision.Column);

                foreach (var i in _cells)
                {
                    ref var cell = ref _cells.Get1(i);
                    FigureAlgorithmFacade.LightUpCellByFigure(figure, cell, position, decision.Direction);
                }
            }
        }

        private static IEnumerable<AiDecision> FindBetterMoves(in bool[,] fillMatrix, ref Figure figure)
        {
            var comparer = new AiMoveVariantComparer();
            var variants = new List<AiMoveVariant>();
            var currentRotation = figure.Rotation;
            foreach (var rotation in FigureAlgorithmFacade.GetRotationVariants(figure))
            {
                figure.Rotation = rotation;
                Analyze(fillMatrix, figure, variants);
            }
            variants.Sort(comparer);

            figure.Rotation = currentRotation;

            if (variants.Count == 0)
                return Enumerable.Repeat(AiDecision.Zero, MOVES_COUNT);
            
            return ChooseBetterMoves(variants, figure);
        }

        private static IEnumerable<AiDecision> ChooseBetterMoves(List<AiMoveVariant> variants, Figure figure)
        {
            var result = new AiDecision[MOVES_COUNT];
            var count = 0;
            foreach (var variant in variants)
            {
                if (count == 0)
                {
                    result[count++] = CreateDecision(variant);
                    Debug.Log(variant.ToString());
                    continue;
                }

                var decision = CreateDecision(variant);
                var isIntersects = false;
                foreach (var aiDecision in result)
                {
                    if (IsIntersects(figure, aiDecision, decision))
                    {
                        isIntersects = true;
                        break;
                    }
                }

                if (isIntersects)
                    continue;
                Debug.Log(variant.ToString());
                result[count++] = decision;
                
                if (count >= MOVES_COUNT)
                    break;
            }

            var temp = result.OrderBy(d => FigureAlgorithmFacade.GetMostLeft(figure, d.Position, d.Rotation)).ToArray();

            for (var i = 0; i < temp.Length; ++i)
            {
                ref var decision = ref temp[i];
                decision.Direction = Directions[i];
            }

            return temp;
        }

        private static bool IsIntersects(in Figure figure, in AiDecision a, in AiDecision b)
        {
            return FigureAlgorithmFacade.IsIntersects(figure, a.Rotation, a.Position, b.Rotation, b.Position);
        }

        private static AiDecision CreateDecision(in AiMoveVariant variant)
        {
            return new AiDecision
            {
                Column = variant.Column,
                Row = variant.Row,
                Rotation = variant.Rotation,
                Direction = Direction.Down
            };
        }

        private static void Analyze(
            bool[,] fillMatrix,
            Figure figure,
            List<AiMoveVariant> variants)
        {
            var rows = fillMatrix.GetLength(0);
            var columns = fillMatrix.GetLength(1);
            // анализируем все столбцы и строки
            for (var row = rows - 1; row >= 0; row--)
            {
                for (var column = 0; column < columns; column++)
                {
                    var place = new GridPosition(row, column);

                    if (!FigureAlgorithmFacade.IsCanPlaceFigure(fillMatrix, figure, place))
                        continue;
                    var actions = Pathfinder.FindPath(figure.Position, place, fillMatrix, figure);
                    if (actions.Count == 0)
                        continue;
                    if (figure.Rotation == FigureRotation.Mirror)
                        actions.AddFirst(PathActions.RotateMirror);

                    if (figure.Rotation == FigureRotation.ClockWise)
                        actions.AddFirst(PathActions.RotateClockwise);

                    if (figure.Rotation == FigureRotation.CounterClockwise)
                        actions.AddFirst(PathActions.RotateCounterClockwise);

                    var variant = new AiMoveVariant
                    {
                        Column = column, Row = row, Rotation = figure.Rotation
                    };

                    var completeLines = FigureAlgorithmFacade.HowManyRowsWillFill(fillMatrix, figure, place);
                    var newFieldState = GridService.CalculateFieldStateAfterCheckLines(fillMatrix);
                    var aggregateHeight = FigureAlgorithmFacade.CalculateNew(newFieldState,
                        figure,
                        place,
                        GridService.CalculateAggregateHeight);
                    var holes = FigureAlgorithmFacade.CalculateNew(newFieldState,
                        figure,
                        place,
                        GridService.CalculateHoles);
                    var bumpiness = FigureAlgorithmFacade.CalculateNew(newFieldState,
                        figure,
                        place,
                        GridService.CalculateBumpiness);
                    variant.AH = aggregateHeight;
                    variant.H = holes;
                    variant.B = bumpiness;
                    variant.Weight = aggregateHeight * AHM + completeLines * CLM + holes * HM + bumpiness * BM;
                    variants.Add(variant);
                }
            }
        }
    }
}