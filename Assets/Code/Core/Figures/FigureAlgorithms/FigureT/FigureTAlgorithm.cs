using System.Collections.Generic;
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
                    FigureRotation.Zero, new FigureTRotationZero()
                },
                {
                    FigureRotation.ClockWise, new FigureTRotationClockwise()
                },
                {
                    FigureRotation.CounterClockwise, new FigureTRotationCounterClockwise()
                },
                {
                    FigureRotation.Mirror, new FigureTRotationMirror()
                }
            };
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
                FigureRotation.Zero,
                FigureRotation.Mirror,
                FigureRotation.ClockWise,
                FigureRotation.CounterClockwise
            };
        }

        public override IEnumerable<GridPosition> GetPositions(in GridPosition place, in Figure figure)
        {
            return _rotatedFigures[figure.Rotation].GetPositions(place);
        }

        protected override bool CheckBordersPlaceFigure(in bool[,] fillMatrix, in Figure figure, in GridPosition place)
        {
            if (!_rotatedFigures[figure.Rotation].CheckBordersForPlaceFigure(fillMatrix, place))
                return false;

            return true;
        }
    }
}