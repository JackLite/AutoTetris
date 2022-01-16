using System.Collections.Generic;
using Core.Grid;

namespace Core.Figures.FigureAlgorithms.FigureZ
{
    /// <summary>
    /// [ ] [*] [ ]
    /// [*] [*] [ ]
    /// [X] [ ] [ ]
    /// </summary>
    public class FigureZRotationClockwise : IRotatedFigure
    {
        public bool CheckBordersForPlaceFigure (in bool[,] fillMatrix, in GridPosition position)
        {
            var rows = fillMatrix.GetLength (0);
            var columns = fillMatrix.GetLength (1);

            if (position.Row == 0 || position.Row > rows - 3 || position.Column > columns - 2)
                return false;

            return true;
        }

        public bool IsFall (in bool[,] fillMatrix, in Figure figure)
        {
            return fillMatrix[figure.Row - 1, figure.Column]
                   || fillMatrix[figure.Row, figure.Column + 1];
        }

        public IEnumerable<GridPosition> GetPositions (in GridPosition position)
        {
            return new[] {
                position,
                position.Above(),
                position.Right().Above(),
                position.Right().Above().Above()
            };
        }
    }
}