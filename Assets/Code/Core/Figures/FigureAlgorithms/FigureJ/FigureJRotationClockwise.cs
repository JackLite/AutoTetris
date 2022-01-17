﻿using System.Collections.Generic;
using Core.Grid;

namespace Core.Figures.FigureAlgorithms.FigureJ
{
    /// <summary>
    /// [ ] [ ] [ ]
    /// [*] [ ] [ ]
    /// [X] [*] [*]
    /// </summary>
    public class FigureJRotationClockwise : IRotatedFigure
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
            return fillMatrix[figure.Row - 1, figure.Column]
                   || fillMatrix[figure.Row - 1, figure.Column + 1]
                   || fillMatrix[figure.Row - 1, figure.Column + 2];
        }

        public GridPosition[] GetPositions(in GridPosition position)
        {
            _positions[0] = position;
            _positions[1] = position.Above();
            _positions[2] = position.Right();
            _positions[3] = position.Right().Right();
            return _positions;
        }
    }
}