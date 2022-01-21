using System.Collections.Generic;
using Core.Cells;
using Core.Grid;

namespace Core.Figures.FigureAlgorithms
{
    public abstract class FigureAlgorithm
    {
        protected readonly Dictionary<FigureRotation, IRotatedFigure> RotatedFigures =
            new Dictionary<FigureRotation, IRotatedFigure>();

        protected readonly List<FigureRotation> FigureRotations = new List<FigureRotation>(4);

        public IEnumerable<FigureRotation> GetRotationVariants()
        {
            return FigureRotations;
        }

        public void SetMatrixValue(in bool[,] fillMatrix, in Figure figure, in GridPosition place, in bool value)
        {
            foreach (var position in GetPositions(place, figure))
            {
                fillMatrix[position.Row, position.Column] = value;
            }
        }

        public void FillGrid(in bool[,] fillMatrix, in Figure figure)
        {
            var place = new GridPosition(figure);
            SetMatrixValue(fillMatrix, figure, place, true);
        }

        public int HowManyRowsWillFill(in bool[,] fillMatrix, in Figure figure, in GridPosition place)
        {
            SetMatrixValue(fillMatrix, figure, place, true);
            var fullRows = GridService.GetFullRowsIndexes(fillMatrix);
            SetMatrixValue(fillMatrix, figure, place, false);

            return fullRows.Count;
        }

        public void CheckAndUpdateCell(in Figure figure, in Cell cell)
        {
            var position = new GridPosition(figure.row, figure.column);

            if (!IsFigureAtCell(position, cell, figure))
                return;

            cell.View.SetImageActive(true);
        }

        public bool IsFigureAtCell(in GridPosition place, in Cell cell, in Figure figure)
        {
            foreach (var pos in GetPositions(place, figure))
            {
                if (pos.Row == cell.Row && pos.Column == cell.Column)
                    return true;
            }

            return false;
        }

        public bool IsCanPlaceFigure(in bool[,] fillMatrix, in Figure figure, in GridPosition place)
        {
            if (!RotatedFigures[figure.rotation].CheckBordersForPlaceFigure(fillMatrix, place))
                return false;

            var positions = GetPositions(place, figure);
            for (var i = 0; i < positions.Length; ++i)
            {
                var pos = positions[i];
                if (pos.Row < 0
                    || pos.Column < 0
                    || pos.Row >= fillMatrix.GetLength(0)
                    || pos.Column >= fillMatrix.GetLength(1))
                    return false;
                if (fillMatrix[pos.Row, pos.Column])
                    return false;
            }

            return true;
        }

        public bool IsFall(in bool[,] fillMatrix, in Figure figure)
        {
            if (figure.row == 0)
                return true;

            if (!RotatedFigures[figure.rotation].CheckBordersForPlaceFigure(fillMatrix, new GridPosition(figure)))
                return false;

            return RotatedFigures[figure.rotation].IsFall(fillMatrix, figure);
        }

        private GridPosition[] GetPositions(in GridPosition place, in Figure figure)
        {
            return RotatedFigures[figure.rotation].GetPositions(place);
        }
        
        public IEnumerable<GridPosition> GetPositions(in GridPosition place, FigureRotation rotation)
        {
            return RotatedFigures[rotation].GetPositions(place);
        }

        public Direction GetBorderDirectionsForCell(in Figure figure, in Cell cell, in GridPosition position)
        {
            var cellPosition = cell.Position;
            return RotatedFigures[figure.rotation].GetBorderDirectionsForCell(cellPosition, position);
        }
    }
}