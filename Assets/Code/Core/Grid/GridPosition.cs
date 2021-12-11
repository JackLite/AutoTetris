using System;
using Core.Figures;

namespace Core.Grid
{
    public readonly struct GridPosition : IEquatable<GridPosition>
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
        public GridPosition To(Direction direction)
        {
            return direction switch
            {
                Direction.Left  => Left(),
                Direction.Down  => Under(),
                Direction.Right => Right(),
                Direction.Top   => Above(),
                _               => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }
        public bool Equals(GridPosition other)
        {
            return Row == other.Row && Column == other.Column;
        }

        public override bool Equals(object obj)
        {
            return obj is GridPosition other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Row * 397) ^ Column;
            }
        }

        public static bool operator !=(GridPosition left, GridPosition right)
        {
            return !Equals(left, right);
        }

        public static bool operator ==(GridPosition left, GridPosition right)
        {
            return Equals(left, right);
        }

        public override string ToString()
        {
            return "Row: " + Row + "; Column: " + Column;
        }
    }
}