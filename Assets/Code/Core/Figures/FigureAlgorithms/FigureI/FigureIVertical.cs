using System.Collections.Generic;
using Core.Grid;

namespace Core.Figures.FigureAlgorithms.FigureI
{
    public class FigureIVertical : IRotatedFigure
    {
        private readonly GridPosition[] _positions;
        public FigureIVertical()
        {
            _positions = new GridPosition[4];
        }
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
            var isFillUnder = fillMatrix[figure.Row - 1, figure.Column];

            return isFillUnder;
        }

        public IEnumerable<GridPosition> GetPositions(in GridPosition position)
        {
            _positions[0] = position;
            _positions[1] = position.Above();
            _positions[2] = position.Above().Above();
            _positions[3] = position.Above().Above().Above();
            return _positions;
        }
    }
}