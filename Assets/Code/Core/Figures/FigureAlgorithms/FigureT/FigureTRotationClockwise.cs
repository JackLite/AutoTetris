using System;
using Core.Grid;

namespace Core.Figures.FigureAlgorithms.FigureT
{
    /// <summary>
    /// [ ] [*] [ ]
    /// [*] [*] [ ]
    /// [X] [*] [ ]
    /// </summary>
    public class FigureTRotationClockwise : IRotatedFigure
    {
        private readonly GridPosition[] _positions = new GridPosition[4];

        public bool CheckBordersForPlaceFigure(in bool[,] fillMatrix, in GridPosition position)
        {
            var rows = fillMatrix.GetLength(0);
            var columns = fillMatrix.GetLength(1);

            if (position.Column > columns - 2 || position.Row > rows - 3)
                return false;

            return true;
        }

        public bool IsFall(in bool[,] fillMatrix, in Figure figure)
        {
            return fillMatrix[figure.row - 1, figure.column + 1] || fillMatrix[figure.row - 1, figure.column];
        }

        public GridPosition[] GetPositions(in GridPosition position)
        {
            _positions[0] = position.Above();
            _positions[1] = _positions[0].Right();
            _positions[2] = _positions[1].Above();
            _positions[3] = _positions[1].Under();
            return _positions;
        }

        public Direction GetBorderDirectionsForCell(in GridPosition cellPosition, in GridPosition position)
        {
            var positions = GetPositions(position);
            if (cellPosition == positions[0])
                return Direction.Bottom | Direction.Left | Direction.Top;
            if (cellPosition == positions[1])
                return Direction.Right;
            if (cellPosition == positions[2])
                return Direction.Top | Direction.Left | Direction.Right;
            if (cellPosition == positions[3])
                return Direction.Bottom | Direction.Left | Direction.Right;
            throw new ArgumentException("Wrong position: " + cellPosition);
        }
    }
}