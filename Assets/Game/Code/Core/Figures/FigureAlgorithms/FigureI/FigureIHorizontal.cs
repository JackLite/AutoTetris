using Core.Grid;

namespace Core.Figures.FigureAlgorithms.FigureI
{
    public class FigureIHorizontal : IRotatedFigure
    {
        private readonly GridPosition[] _positions = new GridPosition[4];

        public bool CheckBordersForPlaceFigure(in bool[,] fillMatrix, in GridPosition position)
        {
            var row = position.Row;
            var column = position.Column;

            if (row > fillMatrix.GetLength(0) || column >= fillMatrix.GetLength(1) - 4)
                return false;

            return true;
        }

        public bool IsFall(in bool[,] fillMatrix, in Figure figure)
        {
            return fillMatrix[figure.row - 1, figure.column]
                   || fillMatrix[figure.row - 1, figure.column + 1]
                   || fillMatrix[figure.row - 1, figure.column + 2]
                   || fillMatrix[figure.row - 1, figure.column + 3];
        }

        public GridPosition[] GetPositions(in GridPosition position)
        {
            _positions[0] = position;
            _positions[1] = position.Right();
            _positions[2] = _positions[1].Right();
            _positions[3] = _positions[2].Right();
            return _positions;
        }

        public Direction GetBorderDirectionsForCell(in GridPosition cellPosition, in GridPosition position)
        {
            var positions = GetPositions(position);
            var result = Direction.Bottom | Direction.Top;
            if (cellPosition == positions[0])
                result |= Direction.Left;
            if (cellPosition == positions[3])
                result |= Direction.Right;
            return result;
        }
    }
}