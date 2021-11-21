using System.Collections.Generic;
using Core.Cells;
using Core.Grid;
using UnityEngine;

namespace Core.Figures.FigureAlgorithms.FigureT
{
    /// <summary>
    /// [*] [*] [*]
    /// [ ] [X] [ ]
    /// </summary>
    public class FigureTRotaionZero : IRotatedFigure
    {
        public bool IsCanPlaceFigure(in bool[,] fillMatrix, in GridPosition position)
        {
            var rows = fillMatrix.GetLength(0);
            var columns = fillMatrix.GetLength(1);

            if (position.Column == 0 || position.Row >= rows - 1 || position.Column >= columns - 1)
                return false;

            foreach (var pos in GetPositions(position))
            {
                if (fillMatrix[pos.Row, pos.Column]) return false;
            }

            return true;
        }

        public void SetMatrixValue(in bool[,] fillMatrix, in GridPosition position, in bool value)
        {
            foreach (var pos in GetPositions(position))
            {
                fillMatrix[pos.Row, pos.Column] = value;
            }
        }

        public bool IsFall(in bool[,] fillMatrix, in Figure figure)
        {
            return fillMatrix[figure.Row - 1, figure.Column]
                   || fillMatrix[figure.Row, figure.Column - 1]
                   || fillMatrix[figure.Row, figure.Column + 1];
        }

        public bool IsFigureAtCell(in GridPosition position, in Cell cell)
        {
            foreach (var pos in GetPositions(position))
            {
                if (pos.Row == cell.Row && pos.Column == cell.Column) return true;
            }

            return false;
        }

        private IEnumerable<GridPosition> GetPositions(in GridPosition position)
        {
            return new[]
            {
                position, position.Left().Above(), position.Above(), position.Right().Above()
            };
        }
    }
}