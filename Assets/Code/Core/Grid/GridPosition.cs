using Core.Figures;

namespace Core.Grid
{
    public struct GridPosition
    {
        public int Row;
        public int Column;

        public static GridPosition Zero => new GridPosition();

        public GridPosition(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public GridPosition(Figure figure)
        {
            Row = figure.Row;
            Column = figure.Column;
        }
    }
}