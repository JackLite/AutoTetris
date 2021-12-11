using System;
using System.Collections.Generic;
using System.Linq;
using Core.Cells;
using Core.Figures;
using Core.Figures.FigureAlgorithms;
using Core.Figures.FigureAlgorithms.Path;
using Core.Grid;
using EcsCore;
using Leopotam.Ecs;
using Unity.Collections;
using UnityEngine;

namespace Core.AI
{
    [EcsSystem(typeof(CoreModule))]
    public class AiSystem : IEcsRunSystem
    {
        private const int MOVES_COUNT = 3;
        private EcsFilter<Figure> _filter;
        private EcsFilter<AiDecision> _decisionsFilter;
        private EcsFilter<Cell> _cells;
        private GridData _gridData;
        private EcsWorld _world;
        private float _timer;

        public void Run()
        {
            if (!_gridData.IsGridStable)
                return;

            if (_filter.GetEntitiesCount() == 0 || _decisionsFilter.GetEntitiesCount() > 0)
            {
                _timer = .25f;
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
                Analyze(fillMatrix, figure, variants, comparer);
            }

            figure.Rotation = currentRotation;

            if (variants.Count == 0)
                return Enumerable.Repeat(AiDecision.Zero, MOVES_COUNT);

            //var aiMoveVariant = variants.Last().Value;
            //return new[] { new AiDecision {Column = aiMoveVariant.Column, Row = aiMoveVariant.Row, Rotation = aiMoveVariant.Rotation} };
            var leftMove = GetBetterMoveForColumns(0, 2, variants);
            var bottomMove = GetBetterMoveForColumns(3, 6, variants);
            var rightMove = GetBetterMoveForColumns(7, 9, variants);

            var result = new List<AiDecision>(MOVES_COUNT);
            UpdateResult(result, leftMove, Direction.Left);
            UpdateResult(result, bottomMove, Direction.Down);
            UpdateResult(result, rightMove, Direction.Right);
            return result;
        }

        private static void UpdateResult(IList<AiDecision> result, AiMoveVariant? variant, Direction direction)
        {
            if (!variant.HasValue)
                return;

            var decision = new AiDecision
            {
                Column = variant.Value.Column, Row = variant.Value.Row, Rotation = variant.Value.Rotation,
                Direction = direction, Path = variant.Value.Path
            };
            result.Add(decision);
        }

        private static AiMoveVariant? GetBetterMoveForColumns(int from, int to, IList<AiMoveVariant> variants)
        {
            for (var i = variants.Count - 1; i >= 0; i--)
            {
                var variant = variants[i];
                if (variant.Column >= from && variant.Column <= to)
                    return variant;
            }
            return null;
        }

        private static void Analyze(
            bool[,] fillMatrix,
            Figure figure,
            List<AiMoveVariant> variants,
            AiMoveVariantComparer comparer)
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
                    
                    var rowsCount = FigureAlgorithmFacade.HowManyRowsWillFill(fillMatrix, figure, place);
                    var lockedCells = FigureAlgorithmFacade.HowManyLockedCellsUnder(fillMatrix, figure, place);
                    var heterogeneity = FigureAlgorithmFacade.CalculateHeterogeneity(fillMatrix, figure, place);
                    var weight = 100 * rowsCount;
                    //weight -=  10 * lockedCells;
                    //weight -= 10 * heterogeneity;
                    weight += 10 * (rows - row);
                    weight += Math.Abs(column - columns / 2);

                    var variant = new AiMoveVariant
                    {
                        Column = column, Row = row, Weight = weight, Rotation = figure.Rotation,
                        H = heterogeneity, FR = rowsCount, Path = actions.ToArray()
                    };

                    variants.Add(variant);
                    variants.Sort(comparer);
                }
            }
        }
    }
}