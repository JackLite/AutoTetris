using System;
using System.Collections.Generic;

namespace Core.Grid
{
    public static class GridService
    {
        public static List<int> GetFullRowsIndexes(bool[,] fillMatrix)
        {
            var rows = new List<int>();
            for (var row = 0; row < fillMatrix.GetLength(0); row++)
            {
                var rowFull = true;

                for (var column = 0; column < fillMatrix.GetLength(1); column++)
                {
                    if (!fillMatrix[row, column])
                    {
                        rowFull = false;

                        break;
                    }
                }

                if (!rowFull)
                    continue;
                rows.Add(row);
            }

            return rows;
        }

        public static bool IsFillSomeAtTopRow(bool[,] fillMatrix)
        {
            var topRow = fillMatrix.GetLength(0) - 3;

            for (var column = 0; column < fillMatrix.GetLength(1); column++)
            {
                if (fillMatrix[topRow, column])
                    return true;
            }

            return false;
        }
        public static int GetLockedCellsUnderFill(in bool[,] fillMatrix)
        {
            var count = 0;
            var rows = fillMatrix.GetLength(0);
            var columns = fillMatrix.GetLength(1);
            for (var row = 0; row < rows - 2; row++)
            {
                var isEmptyRow = true;
                for (var column = 0; column < columns; column++)
                {
                    if (fillMatrix[row, column])
                    {
                        isEmptyRow = false;
                    }
                    var isFillLeft = column == 0 || fillMatrix[row, column - 1];
                    var isFillRight = column == columns - 1 || fillMatrix[row, column + 1];
                    var isFillAbove = fillMatrix[row + 1, column];
                    if (isFillLeft && isFillRight && isFillAbove)
                        count++;
                }

                if (isEmptyRow)
                    break;
            }
            return count;
        }
        public static int GetEmptyCellsUnderFill(bool[,] fillMatrix)
        {
            var count = 0;
            var rows = fillMatrix.GetLength(0);
            var columns = fillMatrix.GetLength(1);
            for (var row = 0; row < rows - 2; row++)
            {
                var isEmptyRow = true;
                for (var column = 0; column < columns; column++)
                {
                    if (fillMatrix[row, column])
                        isEmptyRow = false;
                    else
                        count++;
                }

                if (isEmptyRow)
                {
                    count = Math.Max(0, count - columns);
                    break;
                }
            }
            return count;
        }
        public static int CalculateHeterogeneity(in bool[,] fillMatrix)
        {
            var count = 0;
            var rows = fillMatrix.GetLength(0);
            var columns = fillMatrix.GetLength(1);
            for (var row = 0; row < rows - 2; row++)
            {
                var isEmptyRow = true;
                var isFillCurrent = fillMatrix[row, 0];
                for (var column = 0; column < columns; column++)
                {
                    if (fillMatrix[row, column])
                        isEmptyRow = false;

                    if (isFillCurrent != fillMatrix[row, column])
                        count += 1;

                    isFillCurrent = fillMatrix[row, column];
                }

                if (isFillCurrent)
                    count--;

                if (isEmptyRow)
                    break;
            }
            return count;
        }

        public static bool IsRowEmpty(int rowIndex, bool[,] fillMatrix)
        {
            if (rowIndex >= fillMatrix.GetLength(0))
                return true;

            var columns = fillMatrix.GetLength(1);
            for (var i = 0; i < columns; ++i)
                if (fillMatrix[rowIndex, i])
                    return false;
            return true;
        }
    }
}