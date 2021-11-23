using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Core.Grid;

namespace Core.Figures.FigureAlgorithms.FigureI
{
    public class FigureIHorizontal : IRotatedFigure
    {
        public bool CheckBordersForPlaceFigure(in bool[,] fillMatrix, in GridPosition position)
        {
            var row = position.Row;
            var column = position.Column;

            if (row > fillMatrix.GetLength(0) || column >= fillMatrix.GetLength(1) - 4)
                return false;

            return true;
        }

        public bool IsFall(in bool[,] fillMatrix, in Figure figure)
        {
            return fillMatrix[figure.Row - 1, figure.Column]
                   || fillMatrix[figure.Row - 1, figure.Column + 1]
                   || fillMatrix[figure.Row - 1, figure.Column + 2]
                   || fillMatrix[figure.Row - 1, figure.Column + 3];
        }

        public IEnumerable<GridPosition> GetPositions(in GridPosition position)
        {
            return new[]
            {
                position, position.Right(), position.Right().Right(), position.Right().Right().Right()
            };
        }
    }
}