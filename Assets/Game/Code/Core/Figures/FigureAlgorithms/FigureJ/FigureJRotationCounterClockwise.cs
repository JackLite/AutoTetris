using System;
using Core.Grid;

namespace Core.Figures.FigureAlgorithms.FigureJ
{
    /// <summary>
    /// [ ] [ ] [ ]
    /// [*] [*] [*]
    /// [X] [ ] [*]
    /// </summary>
    public class FigureJRotationCounterClockwise : IRotatedFigure
    {
        private readonly GridPosition[] _positions = new GridPosition[4];

        public bool CheckBordersForPlaceFigure(in bool[,] fillMatrix, in GridPosition position)
        {
            var rows = fillMatrix.GetLength(0);
            var columns = fillMatrix.GetLength(1);

            if (position.Row > rows - 2 || position.Column > columns - 3)
                return false;

            return true;
        }

        public bool IsFall(in bool[,] fillMatrix, in Figure figure)
        {
            return fillMatrix[figure.row, figure.column]
                   || fillMatrix[figure.row, figure.column + 1]
                   || fillMatrix[figure.row - 1, figure.column + 2];
        }

        public GridPosition[] GetPositions(in GridPosition position)
        {
            _positions[0] = position.Above();
            _positions[1] = _positions[0].Right();
            _positions[2] = _positions[1].Right();
            _positions[3] = _positions[2].Under();
            return _positions;
        }

        public Direction GetBorderDirectionsForCell(in GridPosition cellPosition, in GridPosition position)
        {
            var positions = GetPositions(position);
            if (cellPosition == positions[0])
                return Direction.Bottom | Direction.Left | Direction.Top;
            if (cellPosition == positions[1])
                return Direction.Top | Direction.Bottom;
            if (cellPosition == positions[2])
                return Direction.Top | Direction.Right;
            if (cellPosition == positions[3])
                return Direction.Left | Direction.Bottom | Direction.Right;
            throw new ArgumentException("Wrong position: " + cellPosition);
        }
    }
}