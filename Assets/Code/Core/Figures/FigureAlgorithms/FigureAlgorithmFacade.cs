using System.Collections.Generic;
using Core.Cells;
using Core.Grid;
using UnityEngine;

namespace Core.Figures.FigureAlgorithms
{
    public static class FigureAlgorithmFacade
    {
        private static readonly Dictionary<FigureType, IFigureAlgorithm> _algorithms;

        static FigureAlgorithmFacade()
        {
            _algorithms = new Dictionary<FigureType, IFigureAlgorithm>
            {
                {
                    FigureType.O, new FigureO()
                },
                {
                    FigureType.I, new FigureI()
                },
            };
        }

        public static bool IsCanPlaceFigure(in bool[,] fillMatrix, in FigureComponent figure, in GridPosition place)
        {
            var algorithm = _algorithms[figure.Type];

            return algorithm.IsCanPlaceFigure(fillMatrix, place);
        }

        public static void FillGrid(in bool[,] fillMatrix, in FigureComponent figure)
        {
            var algorithm = _algorithms[figure.Type];

            algorithm.FillGrid(fillMatrix, figure);
        }

        public static void UpdateFillCell(in FigureComponent figure, in CellComponent cell)
        {
            var algorithm = _algorithms[figure.Type];

            algorithm.CheckAndUpdateCell(figure, cell);
        }

        public static void LightUpCellByFigure(in FigureComponent figure, in CellComponent cell, in GridPosition place)
        {
            var algorithm = _algorithms[figure.Type];

            algorithm.LightUpCellByFigure(cell, place);
        }

        public static bool IsFall(in bool[,] fillMatrix, in FigureComponent figure)
        {
            var algorithm = _algorithms[figure.Type];
            var gridPosition = new GridPosition(figure.Row, figure.Column);
            return algorithm.IsFall(fillMatrix, gridPosition);
        }
    }
}