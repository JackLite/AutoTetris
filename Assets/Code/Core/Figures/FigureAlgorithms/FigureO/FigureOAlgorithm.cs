using System.Collections.Generic;
using Core.Grid;

namespace Core.Figures.FigureAlgorithms.FigureO
{
    public class FigureOAlgorithm : FigureAlgorithm, IRotatedFigure
    {
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

        public bool IsFall(in bool[,] fillMatrix, in Figure figure)
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

        public IEnumerable<GridPosition> GetPositions(in GridPosition position)
        {
            return new[]
            {
                position, position.Right(), position.Above(), position.Right().Above()
            };
        }
    }
}