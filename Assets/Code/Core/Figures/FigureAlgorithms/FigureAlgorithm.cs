using System.Collections.Generic;
using Core.Cells;
using Core.Grid;

namespace Core.Figures.FigureAlgorithms
{
    public abstract class FigureAlgorithm
    {
        public abstract bool IsFall(in bool[,] fillMatrix, in Figure figure);

        public abstract IEnumerable<FigureRotation> GetRotationVariants();

        public abstract IEnumerable<GridPosition> GetPositions(in GridPosition place, in Figure figure);

        protected abstract bool CheckBordersPlaceFigure(in bool[,] fillMatrix, in Figure figure, in GridPosition place);

        private void SetMatrixValue(in bool[,] fillMatrix, in Figure figure, in GridPosition place, in bool value)
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

        protected bool IsFigureAtCell(in GridPosition place, in Cell cell, in Figure figure)
        {
            foreach (var pos in GetPositions(place, figure))
            {
                if (pos.Row == cell.Row && pos.Column == cell.Column) return true;
            }

            return false;
        }

        public bool IsCanPlaceFigure(in bool[,] fillMatrix, in Figure figure, in GridPosition place)
        {
            if (!CheckBordersPlaceFigure(fillMatrix, figure, place))
                return false;

            foreach (var pos in GetPositions(place, figure))
            {
                if (fillMatrix[pos.Row, pos.Column])
                    return false;
            }

            return true;
        }
        
        public void LightUpCellByFigure(in Cell cell, in Figure figure, in GridPosition place)
        {
            if (!IsFigureAtCell(place, cell, figure))
                return;

            cell.View.LightUp();
        }
    }
}