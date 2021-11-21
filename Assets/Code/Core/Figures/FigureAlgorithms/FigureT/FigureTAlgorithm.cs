using System.Collections.Generic;
using Core.Cells;
using Core.Grid;

namespace Core.Figures.FigureAlgorithms.FigureT
{
    public class FigureTAlgorithm : FigureAlgorithm
    {
        private readonly Dictionary<FigureRotation, IRotatedFigure> _rotatedFigures;

        public FigureTAlgorithm()
        {
            _rotatedFigures = new Dictionary<FigureRotation, IRotatedFigure>
            {
                {
                    FigureRotation.Zero, new FigureTRotaionZero()
                }
            };
        }

        public override bool IsCanPlaceFigure(in bool[,] fillMatrix, in Figure figure, in GridPosition place)
        {
            return _rotatedFigures[figure.Rotation].IsCanPlaceFigure(fillMatrix, place);
        }

        public override void CheckAndUpdateCell(in Figure figure, in Cell cell)
        {
            var position = new GridPosition(figure.Row, figure.Column);

            var rotatedFigure = _rotatedFigures[figure.Rotation];

            if (!rotatedFigure.IsFigureAtCell(position, cell))
                return;

            cell.View.SetImageActive(true);
        }

        public override bool IsFall(in bool[,] fillMatrix, in Figure figure)
        {
            var rows = fillMatrix.GetLength(0);

            if (figure.Row >= rows)
                return false;

            if (figure.Row == 0)
                return true;

            return _rotatedFigures[figure.Rotation].IsFall(fillMatrix, figure);
        }

        public override IEnumerable<FigureRotation> GetRotationVariants()
        {
            return new[]
            {
                FigureRotation.Zero
            };
        }

        public override void LightUpCellByFigure(in Cell cell, in Figure figure, in GridPosition place)
        {
            var rotatedFigure = _rotatedFigures[figure.Rotation];

            if (!rotatedFigure.IsFigureAtCell(place, cell))
                return;

            cell.View.LightUp();
        }

        protected override void SetMatrixValue(
            in bool[,] fillMatrix,
            in Figure figure,
            in GridPosition place,
            in bool value)
        {
            _rotatedFigures[figure.Rotation].SetMatrixValue(fillMatrix, place, value);
        }
    }
}