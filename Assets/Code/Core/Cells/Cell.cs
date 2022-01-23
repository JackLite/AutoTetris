using Core.Cells.Visual;
using Core.Grid;

namespace Core.Cells
{
    public struct Cell
    {
        public int row;
        public int column;
        public CellMono view;

        public GridPosition Position => new GridPosition(row, column);
    }
}