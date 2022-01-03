using Core.Grid;

namespace Core.Cells
{
    public struct Cell
    {
        public int Row;
        public int Column;
        public CellState State;
        public CellMono View;

        public GridPosition Position => new GridPosition(Row, Column);
    }
}