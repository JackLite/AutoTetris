using System.Collections.Generic;
using Core.Cells;
using Core.Grid;

namespace Core.Figures.FigureAlgorithms
{
    public abstract class FigureAlgorithm
    {
        public abstract bool IsCanPlaceFigure(in bool[,] fillMatrix, in Figure figure, in GridPosition place);

        public abstract void CheckAndUpdateCell(in Figure figure, in Cell cell);

        public abstract bool IsFall(in bool[,] fillMatrix, in Figure figure);

        public abstract IEnumerable<FigureRotation> GetRotationVariants();

        public abstract void LightUpCellByFigure(in Cell cell, in Figure figure, in GridPosition place);

        protected abstract void SetMatrixValue(
            in bool[,] fillMatrix,
            in Figure figure,
            in GridPosition place,
            in bool value);

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
    }
}