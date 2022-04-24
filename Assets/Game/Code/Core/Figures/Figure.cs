using System;
using Core.Grid;

namespace Core.Figures
{
    [Serializable]
    public struct Figure
    {
        public FigureType type;
        public int row;
        public int column;
        public FigureRotation rotation;
        public FigureMono mono;

        public GridPosition Position => new GridPosition(row, column);
    }
}