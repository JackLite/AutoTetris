using System.Collections.Generic;
using Core.Cells;
using Core.Figures.FigureAlgorithms.FigureI;
using Core.Figures.FigureAlgorithms.FigureL;
using Core.Figures.FigureAlgorithms.FigureO;
using Core.Figures.FigureAlgorithms.FigureT;
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
        }

        public static bool IsCanPlaceFigure(in bool[,] fillMatrix, in Figure figure, in GridPosition place)
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

        public static void LightUpCellByFigure(in Figure figure, in Cell cell, in GridPosition place)
        {
            var algorithm = _algorithms[figure.Type];

            algorithm.LightUpCellByFigure(cell, figure, place);
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
    }
}