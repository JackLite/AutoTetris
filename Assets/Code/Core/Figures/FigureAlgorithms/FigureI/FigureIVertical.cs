using System.Runtime.CompilerServices;
using Core.Cells;
using Core.Grid;

namespace Core.Figures.FigureAlgorithms.FigureI
{
    public class FigureIVertical : IRotatedFigure
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsCanPlaceFigure(in bool[,] fillMatrix, in GridPosition position)
        {
            var row = position.Row;
            var column = position.Column;

            if (row > fillMatrix.GetLength(0) - 4 || column >= fillMatrix.GetLength(1))
                return false;

            return !fillMatrix[row, column]
                   && !fillMatrix[row + 1, column]
                   && !fillMatrix[row + 2, column]
                   && !fillMatrix[row + 3, column];
        }

        public void SetMatrixValue(in bool[,] fillMatrix, in GridPosition position, in bool value)
        {
            fillMatrix[position.Row, position.Column] = value;
            fillMatrix[position.Row + 1, position.Column] = value;
            fillMatrix[position.Row + 2, position.Column] = value;
            fillMatrix[position.Row + 3, position.Column] = value;
        }

        public bool IsFigureAtCell(in GridPosition position, in Cell cell)
        {
            if (cell.Row < position.Row || cell.Row > position.Row + 3)
                return false;

            return cell.Column == position.Column;
        }

        public bool IsFall(in bool[,] fillMatrix, in Figure figure)
        {
            var isFillUnder = fillMatrix[figure.Row - 1, figure.Column];

            return isFillUnder;
        }
    }
}