using Core.Cells;
using Core.Grid;

namespace Core.Figures.FigureAlgorithms
{
    public interface IRotatedFigure
    {
        bool IsCanPlaceFigure(in bool[,] fillMatrix, in GridPosition position);
        void SetMatrixValue(in bool[,] fillMatrix, in GridPosition position, in bool value);
        bool IsFall(in bool[,] fillMatrix, in Figure figure);
        bool IsFigureAtCell(in GridPosition position, in Cell cell);
    }
}