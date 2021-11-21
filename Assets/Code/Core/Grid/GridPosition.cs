using Core.Figures;

namespace Core.Grid
{
    public readonly struct GridPosition
    {
        public readonly int Row;
        public readonly int Column;

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

        public GridPosition Above()
        {
            return new GridPosition(Row + 1, Column);
        }

        public GridPosition Under()
        {
            return new GridPosition(Row - 1, Column);
        }

        public GridPosition Left()
        {
            return new GridPosition(Row, Column - 1);
        }

        public GridPosition Right()
        {
            return new GridPosition(Row, Column + 1);
        }
    }
}