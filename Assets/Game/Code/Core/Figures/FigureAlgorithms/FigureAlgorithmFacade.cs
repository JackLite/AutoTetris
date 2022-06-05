using System;
using System.Collections.Generic;
using System.Linq;
using Core.AI;
using Core.Cells;
using Core.Figures.FigureAlgorithms.FigureI;
using Core.Figures.FigureAlgorithms.FigureJ;
using Core.Figures.FigureAlgorithms.FigureL;
using Core.Figures.FigureAlgorithms.FigureO;
using Core.Figures.FigureAlgorithms.FigureS;
using Core.Figures.FigureAlgorithms.FigureT;
using Core.Figures.FigureAlgorithms.FigureZ;
using Core.Grid;
using UnityEngine;

namespace Core.Figures.FigureAlgorithms
{
    public static class FigureAlgorithmFacade
    {
        private static readonly Dictionary<FigureType, FigureAlgorithm> _algorithms =
            new Dictionary<FigureType, FigureAlgorithm>();

        static FigureAlgorithmFacade()
        {
            _algorithms.Add(FigureType.O, new FigureOAlgorithm());
            _algorithms.Add(FigureType.I, new FigureIAlgorithm());
            _algorithms.Add(FigureType.T, new FigureTAlgorithm());
            _algorithms.Add(FigureType.L, new FigureLAlgorithm());
            _algorithms.Add(FigureType.J, new FigureJAlgorithm());
            _algorithms.Add(FigureType.Z, new FigureZAlgorithm());
            _algorithms.Add(FigureType.S, new FigureSAlgorithm());
        }

        public static bool IsCanPlaceFigure(in bool[,] fillMatrix, in Figure figure, in GridPosition place)
        {
            var algorithm = _algorithms[figure.type];
            var figureAtPlace = new Figure
            {
                column = place.Column,
                rotation = figure.rotation,
                row = place.Row,
                type = figure.type
            };
            return IsHasSpaceForFigure(fillMatrix, figure, place) && algorithm.IsFall(fillMatrix, figureAtPlace);
        }

        public static bool IsHasSpaceForFigure(in bool[,] fillMatrix, in Figure figure, in GridPosition place)
        {
            var algorithm = _algorithms[figure.type];
            return algorithm.IsCanPlaceFigure(fillMatrix, figure, place);
        }

        public static void FillGrid(in bool[,] fillMatrix, in Figure figure)
        {
            var algorithm = _algorithms[figure.type];

            algorithm.FillGrid(fillMatrix, figure);
        }

        public static void UpdateFillCell(in Figure figure, in Cell cell)
        {
            var algorithm = _algorithms[figure.type];

            algorithm.CheckAndUpdateCell(figure, cell);
        }

        public static bool IsFigureAtCell(in Figure figure, in Cell cell)
        {
            return IsFigureAtCell(figure, cell.Position);
        }

        public static bool IsFigureAtCell(in Figure figure, in Cell cell, in GridPosition figurePosition)
        {
            var algorithm = _algorithms[figure.type];
            return algorithm.IsFigureAtCell(figurePosition, cell, figure);
        }

        private static bool IsFigureAtCell(in Figure figure, in GridPosition cellPosition)
        {
            var algorithm = _algorithms[figure.type];

            var positions = algorithm.GetPositions(figure.Position, figure.rotation);
            foreach (var position in positions)
            {
                if (position == cellPosition)
                    return true;
            }

            return false;
        }

        public static bool IsFall(in bool[,] fillMatrix, in Figure figure)
        {
            var algorithm = _algorithms[figure.type];

            return algorithm.IsFall(fillMatrix, figure);
        }

        public static int HowManyRowsWillFill(in bool[,] fillMatrix, in Figure figure, in GridPosition pos)
        {
            var algorithm = _algorithms[figure.type];

            return algorithm.HowManyRowsWillFill(fillMatrix, figure, pos);
        }

        public static IEnumerable<FigureRotation> GetRotationVariants(in Figure figure)
        {
            var algorithm = _algorithms[figure.type];

            return algorithm.GetRotationVariants();
        }

        public static int CalculateNew(bool[,] fillMatrix, Figure figure, GridPosition place, Func<bool[,], int> calc)
        {
            var algorithm = _algorithms[figure.type];

            algorithm.SetMatrixValue(fillMatrix, figure, place, true);
            var res = calc(fillMatrix);
            algorithm.SetMatrixValue(fillMatrix, figure, place, false);

            return res;
        }

        public static bool IsIntersects(
            in Figure figure,
            FigureRotation firstRotation,
            in GridPosition firstPlace,
            FigureRotation secondRotation,
            in GridPosition secondPlace)
        {
            var algorithm = _algorithms[figure.type];
            var firstPositions = algorithm.GetPositions(firstPlace, firstRotation).ToArray();
            foreach (var pos in algorithm.GetPositions(secondPlace, secondRotation))
            {
                foreach (var firstPos in firstPositions)
                {
                    if (firstPos == pos)
                        return true;
                }
            }

            return false;
        }

        public static int GetMostLeft(in Figure figure, in GridPosition pos, FigureRotation rotation)
        {
            var algorithm = _algorithms[figure.type];
            var res = 100;
            foreach (var position in algorithm.GetPositions(pos, rotation))
            {
                if (position.Column < res)
                    res = position.Column;
            }

            return res;
        }
        
        public static int GetMostRight(in Figure figure, in GridPosition pos, FigureRotation rotation)
        {
            var algorithm = _algorithms[figure.type];
            var res = 0;
            foreach (var position in algorithm.GetPositions(pos, rotation))
            {
                if (position.Column > res)
                    res = position.Column;
            }

            return res;
        }

        public static Direction GetBorderDirectionsForCell(in Figure figure, in Cell cell, in GridPosition position)
        {
            var algorithm = _algorithms[figure.type];
            return algorithm.GetBorderDirectionsForCell(figure, cell, position);
        }

        public static IEnumerable<AiDecision> GetStartDecision(in Figure figure, in Vector2Int gridSize)
        {
            var algorithm = _algorithms[figure.type];
            return algorithm.GetStartDecision(gridSize);
        }
    }
}