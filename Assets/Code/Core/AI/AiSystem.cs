using System;
using System.Collections.Generic;
using System.Linq;
using Core.Cells;
using Core.Figures;
using Core.Figures.FigureAlgorithms;
using Core.Grid;
using EcsCore;
using Leopotam.Ecs;
using UnityEngine;

namespace Core.AI
{
    [EcsSystem(typeof(CoreSetup))]
    public class AiSystem : IEcsRunSystem
    {
        private EcsFilter<Figure>.Exclude<AiDecision> _filter;
        private EcsFilter<Cell> _cells;
        private GridData _gridData;
        private float _timer;

        public void Run()
        {
            if (!_gridData.IsGridStable)
                return;
            
            if (_filter.GetEntitiesCount() == 0)
            {
                _timer = .5f;
                return;
            }

            if (_timer > 0)
            {
                _timer -= Time.deltaTime;

                return;
            }
            ref var figure = ref _filter.Get1(0);

            var aiDecision = FindBetterMove(_gridData.FillMatrix, ref figure);
            _filter.GetEntity(0).Replace(aiDecision);

            LightUpMove(figure, aiDecision);
        }

        private void LightUpMove(Figure figure, in AiDecision aiDecision)
        {
            figure.Rotation = aiDecision.Rotation;
            var position = new GridPosition
            {
                Column = aiDecision.Column, Row = aiDecision.Row
            };

            foreach (var i in _cells)
            {
                ref var cell = ref _cells.Get1(i);
                FigureAlgorithmFacade.LightUpCellByFigure(figure, cell, position);
            }
        }

        private static AiDecision FindBetterMove(in bool[,] fillMatrix, ref Figure figure)
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
                return AiDecision.Zero;

            var result = variants.Last();

            return new AiDecision
            {
                Column = result.Column, Row = result.Row, Rotation = result.Rotation
            };
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

                    var rowsCount = FigureAlgorithmFacade.HowManyRowsWillFill(fillMatrix, figure, place);
                    var weight = 100 * rowsCount;
                    weight += 10 * (rows - row);
                    weight += Math.Abs(column - columns / 2);
                    var variant = new AiMoveVariant
                    {
                        Column = column, Row = row, Weight = weight, Rotation = figure.Rotation
                    };

                    variants.Add(variant);
                    variants.Sort(comparer);
                }
            }
        }
    }
}