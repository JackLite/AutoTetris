using System.Collections.Generic;
using Core.Cells;
using Core.Grid;

namespace Core.Figures.FigureAlgorithms.FigureI
{
    public class FigureIAlgorithm : FigureAlgorithm
    {
        private readonly IEnumerable<FigureRotation> _rotations = new[]
        {
            FigureRotation.Zero, FigureRotation.ClockWise
        };

        private readonly Dictionary<FigureRotation, IRotatedFigure> _rotatedFigures;

        public FigureIAlgorithm()
        {
            _rotatedFigures = new Dictionary<FigureRotation, IRotatedFigure>()
            {
                {
                    FigureRotation.Zero, new FigureIVertical()
                },
                {
                    FigureRotation.ClockWise, new FigureIHorizontal()
                }
            };
        }

        public override IEnumerable<GridPosition> GetPositions(in GridPosition place, in Figure figure)
        {
            return _rotatedFigures[figure.Rotation].GetPositions(place);
        }

        protected override bool CheckBordersPlaceFigure(in bool[,] fillMatrix, in Figure figure, in GridPosition place)
        {
            return _rotatedFigures[figure.Rotation].CheckBordersForPlaceFigure(fillMatrix, place);
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
            return _rotations;
        }
    }
}