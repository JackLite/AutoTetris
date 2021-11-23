using System.Collections.Generic;
using Core.Grid;

namespace Core.Figures.FigureAlgorithms.FigureJ
{
    /// <summary>
    /// [ ] [ ] [*]
    /// [ ] [ ] [*]
    /// [ ] [X] [*]
    /// </summary>
    public class FigureJRotationZero : IRotatedFigure
    {
        public bool CheckBordersForPlaceFigure(in bool[,] fillMatrix, in GridPosition position)
        {
            var rows = fillMatrix.GetLength(0);
            var columns = fillMatrix.GetLength(1);

            if (position.Row > rows - 3 || position.Column > columns - 2)
                return false;

            return true;
        }

        public bool IsFall(in bool[,] fillMatrix, in Figure figure)
        {
            return fillMatrix[figure.Row - 1, figure.Column] || fillMatrix[figure.Row - 1, figure.Column + 1];
        }

        public IEnumerable<GridPosition> GetPositions(in GridPosition position)
        {
            return new[]
            {
                position, position.Right(), position.Right().Above(), position.Right().Above().Above()
            };
        }
    }
}