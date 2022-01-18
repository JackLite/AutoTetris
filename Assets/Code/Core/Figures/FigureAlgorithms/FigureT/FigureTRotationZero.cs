﻿using System;
using Core.Grid;

namespace Core.Figures.FigureAlgorithms.FigureT
{
    /// <summary>
    /// [*] [*] [*]
    /// [ ] [X] [ ]
    /// </summary>
    public class FigureTRotationZero : IRotatedFigure
    {
        private readonly GridPosition[] _positions = new GridPosition[4];

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

        public GridPosition[] GetPositions(in GridPosition position)
        {
            _positions[0] = position;
            _positions[1] = position.Left().Above();
            _positions[2] = position.Above();
            _positions[3] = position.Right().Above();
            return _positions;
        }

        public Direction GetBorderDirectionsForCell(in GridPosition cellPosition, in GridPosition position)
        {
            var positions = GetPositions(position);
            if (cellPosition == positions[0])
                return Direction.Bottom | Direction.Left | Direction.Right;
            if (cellPosition == positions[1])
                return Direction.Left | Direction.Top | Direction.Bottom;
            if (cellPosition == positions[2])
                return Direction.Top;
            if (cellPosition == positions[3])
                return Direction.Top | Direction.Bottom | Direction.Right;
            throw new ArgumentException("Wrong position: " + cellPosition);
        }
    }
}