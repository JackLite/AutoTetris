using System.Collections.Generic;
using Core.Cells;
using Core.Grid;

namespace Core.Figures.FigureAlgorithms.FigureO
{
    public class FigureOAlgorithm : FigureAlgorithm
    {
        private readonly IEnumerable<FigureRotation> _rotations = new[]
        {
            FigureRotation.Zero
        };

        public override IEnumerable<GridPosition> GetPositions(in GridPosition place, in Figure figure)
        {
            return new[]
            {
                place,
                place.Right(),
                place.Above(),
                place.Right().Above()
            };
        }

        protected override bool CheckBordersPlaceFigure(in bool[,] fillMatrix, in Figure figure, in GridPosition place)
        {
            var row = place.Row;
            var column = place.Column;

            if (row > fillMatrix.GetLength(0) - 2 || column > fillMatrix.GetLength(1) - 2)
                return false;

            return true;
        }

        public override bool IsFall(in bool[,] fillMatrix, in Figure figure)
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

        public override IEnumerable<FigureRotation> GetRotationVariants()
        {
            return _rotations;
        }
    }
}