﻿using System.Collections.Generic;
using Core.Grid;

namespace Core.Figures.FigureAlgorithms.FigureL
{
    /// <summary>
    /// [ ] [ ] [ ]
    /// [ ] [ ] [*]
    /// [X] [*] [*]
    /// </summary>
    public class FigureLRotationCounterClockwise : IRotatedFigure
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
            _positions[1] = position.Right();
            _positions[2] = position.Right().Right();
            _positions[3] = position.Right().Right().Above();
            return _positions;
        }

        public Direction GetBorderDirectionsForCell(in GridPosition cellPosition, in GridPosition position)
        {
            throw new System.NotImplementedException();
        }
    }
}