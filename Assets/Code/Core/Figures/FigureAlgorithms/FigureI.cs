using Core.Cells;
using Core.Grid;
using UnityEngine;

namespace Core.Figures.FigureAlgorithms
{
    public class FigureI : IFigureAlgorithm
    {
        public bool IsCanPlaceFigure(in bool[,] fillMatrix, in GridPosition place)
        {
            var row = place.Row;
            var column = place.Column;

            if (row > fillMatrix.GetLength(0) - 4 || column >= fillMatrix.GetLength(1))
                return false;

            return !fillMatrix[row, column]
                   && !fillMatrix[row + 1, column]
                   && !fillMatrix[row + 2, column]
                   && !fillMatrix[row + 3, column];
        }

        public void FillGrid(in bool[,] fillMatrix, in FigureComponent figure)
        {
            fillMatrix[figure.Row, figure.Column] = true;
            fillMatrix[figure.Row + 1, figure.Column] = true;
            fillMatrix[figure.Row + 2, figure.Column] = true;
            fillMatrix[figure.Row + 3, figure.Column] = true;
        }

        public void CheckAndUpdateCell(in FigureComponent figure, in CellComponent cell)
        {
            if (cell.Row < figure.Row || cell.Row > figure.Row + 3)
                return;

            if (cell.Column != figure.Column)
                return;

            cell.View.SetImageActive(true);
        }

        public void LightUpCellByFigure(in CellComponent cell, in GridPosition place)
        {
            if (cell.Row < place.Row || cell.Row > place.Row + 3)
                return;

            if (cell.Column != place.Column)
                return;

            cell.View.LightUp();
        }

        public bool IsFall(in bool[,] fillMatrix, in GridPosition gridPosition)
        {
            var rows = fillMatrix.GetLength(0);

            if (gridPosition.Row >= rows)
                return false;

            if (gridPosition.Row == 0)
                return true;

            var isFillUnder = fillMatrix[gridPosition.Row - 1, gridPosition.Column];

            return isFillUnder;
        }
    }
}