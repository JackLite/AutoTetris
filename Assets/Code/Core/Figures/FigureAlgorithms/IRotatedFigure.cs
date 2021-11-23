using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Core.Grid;

namespace Core.Figures.FigureAlgorithms
{
    public interface IRotatedFigure
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        bool CheckBordersForPlaceFigure(in bool[,] fillMatrix, in GridPosition position);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        bool IsFall(in bool[,] fillMatrix, in Figure figure);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        IEnumerable<GridPosition> GetPositions(in GridPosition position);
    }
}