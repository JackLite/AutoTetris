using System;
using System.Collections.Generic;
using System.Linq;
using Core.Cells;
using Core.Figures.FigureAlgorithms.FigureI;
using Core.Figures.FigureAlgorithms.FigureJ;
using Core.Figures.FigureAlgorithms.FigureL;
using Core.Figures.FigureAlgorithms.FigureO;
using Core.Figures.FigureAlgorithms.FigureS;
using Core.Figures.FigureAlgorithms.FigureT;
using Core.Figures.FigureAlgorithms.FigureZ;
using Core.Grid;

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
            var algorithm = _algorithms[figure.Type];
            var figureAtPlace = new Figure
            {
                Column = place.Column,
                Rotation = figure.Rotation,
                Row = place.Row,
                Type = figure.Type
            };
            return IsHasSpaceForFigure(fillMatrix, figure, place) && algorithm.IsFall(fillMatrix, figureAtPlace);
        }

        public static bool IsHasSpaceForFigure(in bool[,] fillMatrix, in Figure figure, in GridPosition place)
        {
            var algorithm = _algorithms[figure.Type];
            return algorithm.IsCanPlaceFigure(fillMatrix, figure, place);
        }

        public static void FillGrid(in bool[,] fillMatrix, in Figure figure)
        {
            var algorithm = _algorithms[figure.Type];

            algorithm.FillGrid(fillMatrix, figure);
        }

        public static void UpdateFillCell(in Figure figure, in Cell cell)
        {
            var algorithm = _algorithms[figure.Type];

            algorithm.CheckAndUpdateCell(figure, cell);
        }

        public static bool IsFigureAtCell(in Figure figure, in Cell cell)
        {
            var algorithm = _algorithms[figure.Type];

            var positions = algorithm.GetPositions(figure.Position, figure.Rotation);
            foreach (var position in positions)
            {
                if (position == cell.Position)
                    return true;
            }

            return false;
        }

        public static void LightUpCellByFigure(
            in Figure figure,
            in Cell cell,
            in GridPosition place,
            Direction aiDecisionDirection)
        {
            var algorithm = _algorithms[figure.Type];

            algorithm.LightUpCellByFigure(cell, figure, place, aiDecisionDirection);
        }

        public static bool IsFall(in bool[,] fillMatrix, in Figure figure)
        {
            var algorithm = _algorithms[figure.Type];

            return algorithm.IsFall(fillMatrix, figure);
        }

        public static int HowManyRowsWillFill(in bool[,] fillMatrix, in Figure figure, in GridPosition pos)
        {
            var algorithm = _algorithms[figure.Type];

            return algorithm.HowManyRowsWillFill(fillMatrix, figure, pos);
        }

        public static IEnumerable<FigureRotation> GetRotationVariants(in Figure figure)
        {
            var algorithm = _algorithms[figure.Type];

            return algorithm.GetRotationVariants();
        }

        public static int CalculateNew(bool[,] fillMatrix, Figure figure, GridPosition place, Func<bool[,], int> calc)
        {
            var algorithm = _algorithms[figure.Type];

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
            var algorithm = _algorithms[figure.Type];
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
            var algorithm = _algorithms[figure.Type];
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
            var algorithm = _algorithms[figure.Type];
            var res = 0;
            foreach (var position in algorithm.GetPositions(pos, rotation))
            {
                if (position.Column > res)
                    res = position.Column;
            }

            return res;
        }
    }
}