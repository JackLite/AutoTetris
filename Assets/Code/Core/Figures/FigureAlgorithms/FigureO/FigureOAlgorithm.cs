using System.Collections.Generic;
using Core.Grid;

namespace Core.Figures.FigureAlgorithms.FigureO
{
    public class FigureOAlgorithm : FigureAlgorithm, IRotatedFigure
    {
        private readonly GridPosition[] _positions = new GridPosition[4];

        public FigureOAlgorithm()
        {
            RotatedFigures.Add(FigureRotation.Zero, this);

            FigureRotations.Add(FigureRotation.Zero);
        }

        public bool CheckBordersForPlaceFigure(in bool[,] fillMatrix, in GridPosition position)
        {
            var row = position.Row;
            var column = position.Column;

            if (row > fillMatrix.GetLength(0) - 2 || column > fillMatrix.GetLength(1) - 2)
                return false;

            return true;
        }

        bool IRotatedFigure.IsFall(in bool[,] fillMatrix, in Figure figure)
        {
            var rows = fillMatrix.GetLength(0);

            if (figure.Row >= rows)
                return false;

            if (figure.Row == 0)
                return true;

            var isFillUnder = fillMatrix[figure.Row - 1, figure.Column];
            var isFillRightUnder = fillMatrix[figure.Row - 1, figure.Column + 1];

            return isFillUnder || isFillRightUnder;
        }

        public GridPosition[] GetPositions(in GridPosition position)
        {
            _positions[0] = position;
            _positions[1] = position.Right();
            _positions[2] = position.Above();
            _positions[3] = position.Right().Above();
            return _positions;
        }
    }
}