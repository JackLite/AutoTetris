using Core.Cells.Visual;
using Core.Figures;
using Core.Grid;

namespace Core.Cells
{
    public struct Cell
    {
        public int row;
        public int column;
        public FigureType figureType;
        public CellMono view;

        public GridPosition Position => new GridPosition(row, column);
    }
}