using System.Collections.Generic;
using System.Linq;
using Core.AI.Genetic;
using Core.Cells;
using Core.Figures;
using Core.Figures.FigureAlgorithms;
using Core.Grid;
using Core.Moving;
using Core.Path;
using EcsCore;
using Global.Settings.Core;
using Leopotam.Ecs;
using UnityEngine;

namespace Core.AI
{
    [EcsSystem(typeof(CoreModule))]
    public class AiSystem : IEcsPreInitSystem, IEcsRunSystem
    {
        private const float AHM = -4.96598f;
        private const float CLM = 2.994397f;
        private const float HM = -5.276442f;
        private const float BM = -1.619069f;

        private static readonly Dictionary<int, Direction> directions = new Dictionary<int, Direction>
        {
            { 0, Direction.Left },
            { 1, Direction.Bottom },
            { 2, Direction.Right },
        };

        private EcsFilter<Figure>.Exclude<FigureMoveChosen> _filter;
        private EcsFilter<AiDecision> _decisionsFilter;
        private EcsFilter<Cell> _cells;
        private GridData _gridData;
        private EcsWorld _world;
        private float _timer;
        private int _movesCount;
        private CoreState _coreState;
        private AiGeneticService _genetic;
        private CoreSettings _settings;

        public void PreInit()
        {
            _movesCount = _settings.aiEnable ? 1 : 3;
        }

        public void Run()
        {
            if (!_gridData.IsGridStable || _coreState.IsPaused)
                return;

            if (_filter.GetEntitiesCount() == 0 || _decisionsFilter.GetEntitiesCount() > 0)
            {
                _timer = .06f;
                return;
            }

            if (_timer > 0 && !_settings.aiEnable)
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
        }

        private IEnumerable<AiDecision> FindBetterMoves(bool[,] fillMatrix, ref Figure figure)
        {
            if (GridService.IsGridEmpty(fillMatrix))
            {
                var size = new Vector2Int(fillMatrix.GetLength(0), fillMatrix.GetLength(1));
                return FigureAlgorithmFacade.GetStartDecision(figure, size);
            }

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
                return Enumerable.Repeat(AiDecision.Zero, _movesCount);

            return ChooseBetterMoves(variants, figure);
        }

        private IEnumerable<AiDecision> ChooseBetterMoves2(List<AiMoveVariant> variants, Figure figure)
        {
            var result = new AiDecision[_movesCount];
            var count = 0;
            foreach (var variant in variants)
            {
                if (variant.Column > 2)
                    continue;
                var targetPoint = new GridPosition(variant.Row, variant.Column);
                figure.rotation = variant.Rotation;
                if (FigurePathfinder.FindPath(figure.Position, targetPoint, _gridData.FillMatrix, figure).Count == 0)
                    continue;
                result[count++] = CreateDecision(variant);
                break;
            }

            foreach (var variant in variants)
            {
                if (variant.Column < 3 || variant.Column > 6)
                    continue;
                var targetPoint = new GridPosition(variant.Row, variant.Column);
                figure.rotation = variant.Rotation;
                if (FigurePathfinder.FindPath(figure.Position, targetPoint, _gridData.FillMatrix, figure).Count == 0)
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
                if (FigurePathfinder.FindPath(figure.Position, targetPoint, _gridData.FillMatrix, figure).Count == 0)
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
                decision.Direction = directions[i];
            }
            return result;
        }

        private IEnumerable<AiDecision> ChooseBetterMoves(List<AiMoveVariant> variants, Figure figure)
        {
            var result = new AiDecision[_movesCount];
            var count = 0;
            foreach (var variant in variants)
            {
                var targetPoint = new GridPosition(variant.Row, variant.Column);
                figure.rotation = variant.Rotation;
                if (FigurePathfinder.FindPath(figure.Position, targetPoint, _gridData.FillMatrix, figure).Count == 0)
                    continue;
                if (count == 0)
                {
                    result[count++] = CreateDecision(variant);
                    if (count >= _movesCount)
                        break;
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
                if (count >= _movesCount)
                    break;
            }
            figure.rotation = FigureRotation.Zero;

            var temp = result.OrderBy(d => FigureAlgorithmFacade.GetMostLeft(figure, d.Position, d.Rotation)).ToArray();

            for (var i = 0; i < temp.Length; ++i)
            {
                ref var decision = ref temp[i];
                if (decision.Direction == Direction.None)
                    continue;
                decision.Direction = directions[i];
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

        private void Analyze(bool[,] fillMatrix, Figure figure, List<AiMoveVariant> variants)
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
                    if (_settings.aiEnable)
                    {
                        var ind = _genetic.currentIndividual;
                        variant.Weight = aggregateHeight * ind.ah
                                         + completeLines * ind.lines
                                         + holes * ind.holes
                                         + bumpiness * ind.bumpiness;
                    }
                    else
                    {
                        variant.Weight = aggregateHeight * AHM + completeLines * CLM + holes * HM + bumpiness * BM;
                    }
                    variants.Add(variant);
                }
            }
        }
    }
}