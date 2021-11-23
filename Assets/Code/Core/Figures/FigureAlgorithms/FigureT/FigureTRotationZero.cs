using System.Collections.Generic;
using Core.Grid;

namespace Core.Figures.FigureAlgorithms.FigureT
{
    /// <summary>
    /// [*] [*] [*]
    /// [ ] [X] [ ]
    /// </summary>
    public class FigureTRotationZero : IRotatedFigure
    {
        public bool CheckBordersForPlaceFigure(in bool[,] fillMatrix, in GridPosition position)
        {
            var rows = fillMatrix.GetLength(0);
            var columns = fillMatrix.GetLength(1);

            if (position.Column == 0 || position.Row >= rows - 1 || position.Column >= columns - 1)
                return false;

            return true;
        }

        public bool IsFall(in bool[,] fillMatrix, in Figure figure)
        {
            return fillMatrix[figure.Row - 1, figure.Column]
                   || fillMatrix[figure.Row, figure.Column - 1]
                   || fillMatrix[figure.Row, figure.Column + 1];
        }

        public IEnumerable<GridPosition> GetPositions(in GridPosition position)
        {
            return new[]
            {
                position, position.Left().Above(), position.Above(), position.Right().Above()
            };
        }
    }
}