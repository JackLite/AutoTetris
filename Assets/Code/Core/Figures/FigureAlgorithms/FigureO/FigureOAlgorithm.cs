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

        public override bool IsCanPlaceFigure(in bool[,] fillMatrix, in Figure figure, in GridPosition place)
        {
            var row = place.Row;
            var column = place.Column;

            if (row > fillMatrix.GetLength(0) - 2 || column > fillMatrix.GetLength(1) - 2)
                return false;

            return !fillMatrix[row, column]
                   && !fillMatrix[row + 1, column]
                   && !fillMatrix[row, column + 1]
                   && !fillMatrix[row + 1, column + 1];
        }

        protected override void SetMatrixValue(in bool[,] fillMatrix, in Figure _, in GridPosition place, in bool value)
        {
            fillMatrix[place.Row, place.Column] = value;
            fillMatrix[place.Row + 1, place.Column] = value;
            fillMatrix[place.Row, place.Column + 1] = value;
            fillMatrix[place.Row + 1, place.Column + 1] = value;
        }

        public override void CheckAndUpdateCell(in Figure figure, in Cell cell)
        {
            if (cell.Row != figure.Row && cell.Row != figure.Row + 1)
                return;

            if (cell.Column != figure.Column && cell.Column != figure.Column + 1)
                return;

            cell.View.SetImageActive(true);
        }

        public override void LightUpCellByFigure(in Cell cell, in Figure figure, in GridPosition place)
        {
            if (cell.Row != place.Row && cell.Row != place.Row + 1)
                return;

            if (cell.Column != place.Column && cell.Column != place.Column + 1)
                return;

            cell.View.LightUp();
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