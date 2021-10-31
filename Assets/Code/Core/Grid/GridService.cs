using Core.Figures;

namespace Core.Grid
{
    public static class GridService
    {
        public static bool IsCanPlaceFigure(in bool[,] fillMatrix, int row, int column)
        {
            if (row > fillMatrix.GetLength(0) - 2 || column > fillMatrix.GetLength(1) - 2)
                return false;

            return !fillMatrix[row, column]
                   && !fillMatrix[row + 1, column]
                   && !fillMatrix[row, column + 1]
                   && !fillMatrix[row + 1, column + 1];
        }

        public static void FillGrid(in bool[,] fillMatrix, in FigureComponent figure)
        {
            fillMatrix[figure.Row, figure.Column] = true;
            fillMatrix[figure.Row + 1, figure.Column] = true;
            fillMatrix[figure.Row, figure.Column + 1] = true;
            fillMatrix[figure.Row + 1, figure.Column + 1] = true;
        }
    }
}