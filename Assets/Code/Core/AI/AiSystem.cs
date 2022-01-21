using System.Collections.Generic;
using System.Linq;
using Core.Cells;
using Core.Figures;
using Core.Figures.FigureAlgorithms;
using Core.Grid;
using Core.Path;
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
            {0, Direction.Left},
            {1, Direction.Bottom},
            {2, Direction.Right},
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
                _timer = .06f;
                return;
            }

            if (_timer > 0)
            {
                _timer -= Time.deltaTime;

                return;
            }

            ref var figure = ref _filter.Get1(0);

            if (figure.row > 20)
                return;

            var aiDecisions = FindBetterMoves(_gridData.FillMatrix, ref figure).ToArray();
            foreach (var decision in aiDecisions)
            {
                if (decision.Direction == Direction.None)
                    continue;
                _world.NewEntity().Replace(decision);
            }

            LightUpMoves(figure, aiDecisions);
        }

        private void LightUpMoves(Figure figure, in IEnumerable<AiDecision> aiDecisions)
        {
            foreach (var i in _cells)
            {
                ref var cell = ref _cells.Get1(i);
                var needLightDown = true;
                foreach (var decision in aiDecisions)
                {
                    if (decision.Direction == Direction.None)
                        continue;
                    figure.rotation = decision.Rotation;
                    var position = new GridPosition(decision.Row, decision.Column);

                    if (FigureAlgorithmFacade.IsFigureAtCell(figure, cell, position))
                    {
                        cell.View.LightUp(figure, decision.Direction);
                        var directionMask = FigureAlgorithmFacade.GetBorderDirectionsForCell(figure, cell, position);
                        cell.View.ShowBorders(directionMask);
                        needLightDown = false;
                        break;
                    }
                }
                if (needLightDown)
                    cell.View.LightDown();
            }
        }

        private IEnumerable<AiDecision> FindBetterMoves(in bool[,] fillMatrix, ref Figure figure)
        {
            var comparer = new AiMoveVariantComparer();
            var variants = new List<AiMoveVariant>();
            var currentRotation = figure.rotation;
            foreach (var rotation in FigureAlgorithmFacade.GetRotationVariants(figure))
            {
                figure.rotation = rotation;
                Analyze(fillMatrix, figure, variants);
            }

            variants.Sort(comparer);

            figure.rotation = currentRotation;

            if (variants.Count == 0)
                return Enumerable.Repeat(AiDecision.Zero, MOVES_COUNT);

            return ChooseBetterMoves(variants, figure);
        }

        private IEnumerable<AiDecision> ChooseBetterMoves2(List<AiMoveVariant> variants, Figure figure)
        {
            var result = new AiDecision[MOVES_COUNT];
            var count = 0;
            foreach (var variant in variants)
            {
                if (variant.Column > 2)
                    continue;
                var targetPoint = new GridPosition(variant.Row, variant.Column);
                figure.rotation = variant.Rotation;
                if (Pathfinder.FindPath(figure.Position, targetPoint, _gridData.FillMatrix, figure).Count == 0)
                    continue;
                result[count++] = CreateDecision(variant);
                break;
            }
            
            foreach (var variant in variants)
            {
                if (variant.Column < 3 ||variant.Column > 6)
                    continue;
                var targetPoint = new GridPosition(variant.Row, variant.Column);
                figure.rotation = variant.Rotation;
                if (Pathfinder.FindPath(figure.Position, targetPoint, _gridData.FillMatrix, figure).Count == 0)
                    continue;
                var decision = CreateDecision(variant);
                var isIntersects = false;
                foreach (var aiDecision in result)
                {
                    if (aiDecision.Direction == Direction.None)
                        continue;
                    if (IsIntersects(figure, aiDecision, decision))
                    {
                        isIntersects = true;
                        break;
                    }
                }

                if (isIntersects)
                    continue;
                result[count++] = decision;
                break;
            }
            
            foreach (var variant in variants)
            {
                if (variant.Column < 7)
                    continue;
                var targetPoint = new GridPosition(variant.Row, variant.Column);
                figure.rotation = variant.Rotation;
                if (Pathfinder.FindPath(figure.Position, targetPoint, _gridData.FillMatrix, figure).Count == 0)
                    continue;
                var decision = CreateDecision(variant);
                var isIntersects = false;
                foreach (var aiDecision in result)
                {
                    if (aiDecision.Direction == Direction.None)
                        continue;
                    if (IsIntersects(figure, aiDecision, decision))
                    {
                        isIntersects = true;
                        break;
                    }
                }

                if (isIntersects)
                    continue;
                result[count] = decision;
                break;
            }
            
            figure.rotation = FigureRotation.Zero;


            for (var i = 0; i < result.Length; ++i)
            {
                ref var decision = ref result[i];
                if (decision.Direction == Direction.None)
                    continue;
                decision.Direction = Directions[i];
            }
            return result;
        }
        
        private IEnumerable<AiDecision> ChooseBetterMoves(List<AiMoveVariant> variants, Figure figure)
        {
            var result = new AiDecision[MOVES_COUNT];
            var count = 0;
            foreach (var variant in variants)
            {
                var targetPoint = new GridPosition(variant.Row, variant.Column);
                figure.rotation = variant.Rotation;
                if (Pathfinder.FindPath(figure.Position, targetPoint, _gridData.FillMatrix, figure).Count == 0)
                    continue;
                if (count == 0)
                {
                    result[count++] = CreateDecision(variant);
                    continue;
                }

                var decision = CreateDecision(variant);
                var isIntersects = false;
                foreach (var aiDecision in result)
                {
                    if (aiDecision.Direction == Direction.None)
                        continue;
                    if (IsIntersects(figure, aiDecision, decision))
                    {
                        isIntersects = true;
                        break;
                    }
                }

                if (isIntersects)
                    continue;
                result[count++] = decision;

                if (count >= MOVES_COUNT)
                    break;
            }
            figure.rotation = FigureRotation.Zero;

            var temp = result.OrderBy(d => FigureAlgorithmFacade.GetMostLeft(figure, d.Position, d.Rotation)).ToArray();

            for (var i = 0; i < temp.Length; ++i)
            {
                ref var decision = ref temp[i];
                if (decision.Direction == Direction.None)
                    continue;
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
                Direction = Direction.Bottom
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
                    var variant = new AiMoveVariant
                    {
                        Column = column, Row = row, Rotation = figure.rotation
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