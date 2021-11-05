using System.Runtime.CompilerServices;
using Core.Cells;
using Core.Grid;

namespace Core.Figures.FigureAlgorithms.FigureI
{
    public class FigureIHorizontal : IRotatedFigure
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsCanPlaceFigure(in bool[,] fillMatrix, in GridPosition position)
        {
            var row = position.Row;
            var column = position.Column;

            if (row > fillMatrix.GetLength(0) || column >= fillMatrix.GetLength(1) - 4)
                return false;

            return !fillMatrix[row, column]
                   && !fillMatrix[row, column + 1]
                   && !fillMatrix[row, column + 2]
                   && !fillMatrix[row, column + 3];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetMatrixValue(in bool[,] fillMatrix, in GridPosition position, in bool value)
        {
            fillMatrix[position.Row, position.Column] = value;
            fillMatrix[position.Row, position.Column + 1] = value;
            fillMatrix[position.Row, position.Column + 2] = value;
            fillMatrix[position.Row, position.Column + 3] = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsFall(in bool[,] fillMatrix, in Figure figure)
        {
            return fillMatrix[figure.Row - 1, figure.Column]
                || fillMatrix[figure.Row - 1, figure.Column + 1]
                || fillMatrix[figure.Row - 1, figure.Column + 2]
                || fillMatrix[figure.Row - 1, figure.Column + 3];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsFigureAtCell(in GridPosition position, in Cell cell)
        {
            if (cell.Row != position.Row)
                return false;

            if (cell.Column < position.Column || cell.Column > position.Column + 3)
                return false;

            return true;
        }
    }
}