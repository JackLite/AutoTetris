using Core.Grid;

namespace Core.Figures.FigureAlgorithms.FigureI
{
    public class FigureIVertical : IRotatedFigure
    {
        private readonly GridPosition[] _positions = new GridPosition[4];

        public bool CheckBordersForPlaceFigure(in bool[,] fillMatrix, in GridPosition position)
        {
            var row = position.Row;
            var column = position.Column;

            if (row > fillMatrix.GetLength(0) - 4 || column >= fillMatrix.GetLength(1))
                return false;

            return true;
        }

        public bool IsFall(in bool[,] fillMatrix, in Figure figure)
        {
            var isFillUnder = fillMatrix[figure.row - 1, figure.column];

            return isFillUnder;
        }

        public GridPosition[] GetPositions(in GridPosition position)
        {
            _positions[0] = position;
            _positions[1] = position.Above();
            _positions[2] = _positions[1].Above();
            _positions[3] = _positions[2].Above();
            return _positions;
        }

        public Direction GetBorderDirectionsForCell(in GridPosition cellPosition, in GridPosition position)
        {
            var positions = GetPositions(position);
            var result = Direction.Left | Direction.Right;
            if (cellPosition == positions[0])
                result |= Direction.Bottom;
            if (cellPosition == positions[3])
                result |= Direction.Top;
            return result;
        }
    }
}