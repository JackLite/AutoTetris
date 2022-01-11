using System.Collections.Generic;
using Core.AI;
using Core.Cells;
using Core.Grid;
using UnityEngine;

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
            var position = new GridPosition(figure.Row, figure.Column);

            if (!IsFigureAtCell(position, cell, figure))
                return;

            cell.View.SetImageActive(true);
        }

        private bool IsFigureAtCell(in GridPosition place, in Cell cell, in Figure figure)
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
            if (!RotatedFigures[figure.Rotation].CheckBordersForPlaceFigure(fillMatrix, place))
                return false;

            foreach (var pos in GetPositions(place, figure))
            {
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

        public void LightUpCellByFigure(
            in Cell cell,
            in Figure figure,
            in GridPosition place,
            Direction aiDecisionDirection)
        {
            if (!IsFigureAtCell(place, cell, figure))
                return;

            cell.View.LightUp(figure, aiDecisionDirection);
        }

        public bool IsFall(in bool[,] fillMatrix, in Figure figure)
        {
            if (figure.Row == 0)
                return true;

            if (!RotatedFigures[figure.Rotation].CheckBordersForPlaceFigure(fillMatrix, new GridPosition(figure)))
                return false;

            return RotatedFigures[figure.Rotation].IsFall(fillMatrix, figure);
        }

        private IEnumerable<GridPosition> GetPositions(in GridPosition place, in Figure figure)
        {
            return RotatedFigures[figure.Rotation].GetPositions(place);
        }
        
        public IEnumerable<GridPosition> GetPositions(in GridPosition place, FigureRotation rotation)
        {
            return RotatedFigures[rotation].GetPositions(place);
        }
    }
}