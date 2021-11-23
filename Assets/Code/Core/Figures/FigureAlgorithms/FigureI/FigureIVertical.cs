using System.Collections.Generic;
using Core.Grid;

namespace Core.Figures.FigureAlgorithms.FigureI
{
    public class FigureIVertical : IRotatedFigure
    {
        public bool CheckBordersForPlaceFigure(in bool[,] fillMatrix, in GridPosition position)
        {
            var row = position.Row;
            var column = position.Column;

            if (row > fillMatrix.GetLength(0) - 4 || column >= fillMatrix.GetLength(1))
                return false;

            return true;
        }

        public bool IsFall(in bool[,] fillMatrix, in Figure figure)
        {
            var isFillUnder = fillMatrix[figure.Row - 1, figure.Column];

            return isFillUnder;
        }

        public IEnumerable<GridPosition> GetPositions(in GridPosition position)
        {
            return new[]
            {
                position, position.Above(), position.Above().Above(), position.Above().Above().Above()
            };
        }
    }
}