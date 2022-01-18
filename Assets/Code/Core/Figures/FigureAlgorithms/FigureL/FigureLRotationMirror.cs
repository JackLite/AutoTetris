using System;
using Core.Grid;

namespace Core.Figures.FigureAlgorithms.FigureL
{
    /// <summary>
    /// [X] [*] [ ]
    /// [ ] [*] [ ]
    /// [ ] [*] [ ]
    /// </summary>
    public class FigureLRotationMirror : IRotatedFigure
    {
        private readonly GridPosition[] _positions = new GridPosition[4];

        public bool CheckBordersForPlaceFigure(in bool[,] fillMatrix, in GridPosition position)
        {
            var rows = fillMatrix.GetLength(0);
            var columns = fillMatrix.GetLength(1);

            if (position.Row < 3 || position.Row > rows - 1 || position.Column > columns - 2)
                return false;

            return true;
        }

        public bool IsFall(in bool[,] fillMatrix, in Figure figure)
        {
            return fillMatrix[figure.Row - 1, figure.Column] 
                   || fillMatrix[figure.Row - 3, figure.Column + 1];
        }

        public GridPosition[] GetPositions(in GridPosition position)
        {
            _positions[0] = position;
            _positions[1] = position.Right();
            _positions[2] = position.Right().Under();
            _positions[3] = position.Right().Under().Under();
            return _positions;
        }

        public Direction GetBorderDirectionsForCell(in GridPosition cellPosition, in GridPosition position)
        {
            var positions = GetPositions(position);
            if (cellPosition == positions[0])
                return Direction.Bottom | Direction.Left | Direction.Top;
            if (cellPosition == positions[1])
                return Direction.Top | Direction.Right;
            if (cellPosition == positions[2])
                return Direction.Left | Direction.Right;
            if (cellPosition == positions[3])
                return Direction.Left | Direction.Bottom | Direction.Right;
            throw new ArgumentException("Wrong position: " + cellPosition);
        }
    }
}