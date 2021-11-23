using System.Collections.Generic;
using Core.Grid;

namespace Core.Figures.FigureAlgorithms.FigureS
{
    /// <summary>
    /// [X] [ ] [ ]
    /// [*] [*] [ ]
    /// [ ] [*] [ ]
    /// </summary>
    public class FigureSRotationClockwise : IRotatedFigure
    {
        public bool CheckBordersForPlaceFigure (in bool[,] fillMatrix, in GridPosition position)
        {
            var rows = fillMatrix.GetLength (0);
            var columns = fillMatrix.GetLength (1);

            if (position.Row < 2 || position.Row > rows - 1 || position.Column > columns - 2)
                return false;

            return true;
        }

        public bool IsFall (in bool[,] fillMatrix, in Figure figure)
        {
            return fillMatrix[figure.Row - 2, figure.Column]
                   || fillMatrix[figure.Row - 3, figure.Column + 1];
        }

        public IEnumerable<GridPosition> GetPositions (in GridPosition position)
        {
            return new[] {
                position,
                position.Under(),
                position.Right().Under(),
                position.Right().Under().Under()
            };
        }
    }
}