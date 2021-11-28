using System.Collections.Generic;
using Core.Grid;

namespace Core.Figures.FigureAlgorithms.FigureZ
{
    /// <summary>
    /// [ ] [ ] [ ]
    /// [X] [*] [ ]
    /// [ ] [*] [*]
    /// </summary>
    public class FigureZRotationZero : IRotatedFigure
    {
        public bool CheckBordersForPlaceFigure(in bool[,] fillMatrix, in GridPosition position)
        {
            var rows = fillMatrix.GetLength(0);
            var columns = fillMatrix.GetLength(1);

            if (position.Row == 0 || position.Row > rows - 1 || position.Column > columns - 3)
                return false;

            return true;
        }

        public bool IsFall(in bool[,] fillMatrix, in Figure figure)
        {
            if (figure.Row == 1)
                return true;

            return fillMatrix[figure.Row - 1, figure.Column]
                   || fillMatrix[figure.Row - 2, figure.Column + 1]
                   || fillMatrix[figure.Row - 2, figure.Column + 2];
        }

        public IEnumerable<GridPosition> GetPositions(in GridPosition position)
        {
            return new[]
            {
                position,
                position.Right(),
                position.Right().Under(),
                position.Right().Right().Under()
            };
        }
    }
}