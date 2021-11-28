using System.Collections.Generic;
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
            var isCanPlaceFigure = algorithm.IsCanPlaceFigure(fillMatrix, figure, place);
            return isCanPlaceFigure && algorithm.IsFall(fillMatrix, figureAtPlace);
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
        public static int HowManyLockedCellsUnder(bool[,] fillMatrix, Figure figure, GridPosition place)
        {
            var algorithm = _algorithms[figure.Type];

            return algorithm.HowManyLockedCellsUnder(fillMatrix, figure, place);
        }
        public static int HowManyEmptyCellsUnder(bool[,] fillMatrix, Figure figure, GridPosition place)
        {
            var algorithm = _algorithms[figure.Type];

            return algorithm.HowManyEmptyCellsUnder(fillMatrix, figure, place);
        }
        public static int CalculateHeterogeneity(bool[,] fillMatrix, Figure figure, GridPosition place)
        {
            var algorithm = _algorithms[figure.Type];

            var old = GridService.CalculateHeterogeneity(fillMatrix);
            algorithm.SetMatrixValue(fillMatrix, figure, place, true);
            var heterogeneity = GridService.CalculateHeterogeneity(fillMatrix);
            algorithm.SetMatrixValue(fillMatrix, figure, place, false);

            return heterogeneity - old;
        }
    }
}