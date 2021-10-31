using Core.Cells;
using Core.Grid;
using UnityEngine;

namespace Core.Figures.FigureAlgorithms
{
    public interface IFigureAlgorithm
    {
        bool IsCanPlaceFigure(in bool[,] fillMatrix, in GridPosition place);

        void FillGrid(in bool[,] fillMatrix, in FigureComponent figure);
        
        void CheckAndUpdateCell(in FigureComponent figure, in CellComponent cell);
        void LightUpCellByFigure(in CellComponent cell, in GridPosition place);
        bool IsFall(in bool[,] fillMatrix, in GridPosition gridPosition);
    }
}