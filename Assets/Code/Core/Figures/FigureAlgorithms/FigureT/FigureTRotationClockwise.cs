using System.Collections.Generic;
using Core.Grid;

namespace Core.Figures.FigureAlgorithms.FigureT
{
    /// <summary>
    /// [ ] [ ] [*]
    /// [ ] [X] [*]
    /// [ ] [ ] [*]
    /// </summary>
    public class FigureTRotationClockwise : IRotatedFigure
    {
        public bool CheckBordersForPlaceFigure(in bool[,] fillMatrix, in GridPosition position)
        {
            var rows = fillMatrix.GetLength(0);
            var columns = fillMatrix.GetLength(1);

            if (position.Column > columns - 2 || position.Row == 0 || position.Row > rows - 2)
                return false;

            return true;
        }

        public bool IsFall(in bool[,] fillMatrix, in Figure figure)
        {
            return fillMatrix[figure.Row - 2, figure.Column + 1]
                   || fillMatrix[figure.Row - 1, figure.Column];
        }

        public IEnumerable<GridPosition> GetPositions(in GridPosition position)
        {
            return new[]
            {
                position, 
                position.Right().Above(), 
                position.Right(), 
                position.Right().Under()
            };
        }
    }
}