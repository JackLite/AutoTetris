using Core.Cells;
using Core.Grid;
using UnityEngine;

namespace Core.Figures.FigureAlgorithms
{
    public class FigureO : IFigureAlgorithm
    {
        public bool IsCanPlaceFigure(in bool[,] fillMatrix, in GridPosition place)
        {
            var row = place.Row;
            var column = place.Column;

            if (row > fillMatrix.GetLength(0) - 2 || column > fillMatrix.GetLength(1) - 2)
                return false;

            return !fillMatrix[row, column]
                   && !fillMatrix[row + 1, column]
                   && !fillMatrix[row, column + 1]
                   && !fillMatrix[row + 1, column + 1];
        }

        public void FillGrid(in bool[,] fillMatrix, in FigureComponent figure)
        {
            fillMatrix[figure.Row, figure.Column] = true;
            fillMatrix[figure.Row + 1, figure.Column] = true;
            fillMatrix[figure.Row, figure.Column + 1] = true;
            fillMatrix[figure.Row + 1, figure.Column + 1] = true;
        }

        public void CheckAndUpdateCell(in FigureComponent figure, in CellComponent cell)
        {
            if (cell.Row != figure.Row && cell.Row != figure.Row + 1)
                return;

            if (cell.Column != figure.Column && cell.Column != figure.Column + 1)
                return;

            cell.View.SetImageActive(true);
        }
        
        public void LightUpCellByFigure(in CellComponent cell, in GridPosition place)
        {
            if (cell.Row != place.Row && cell.Row != place.Row + 1)
                return;

            if (cell.Column != place.Column && cell.Column != place.Column + 1)
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
            var isFillRightUnder = fillMatrix[gridPosition.Row - 1, gridPosition.Column + 1];

            return isFillUnder || isFillRightUnder;
        }
    }
}