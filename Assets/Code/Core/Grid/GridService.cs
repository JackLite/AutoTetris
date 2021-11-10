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
            var topRow = fillMatrix.GetLength(0) - 1;

            for (var column = 0; column < fillMatrix.GetLength(1); column++)
            {
                if (fillMatrix[topRow, column])
                    return true;
            }

            return false;
        }
    }
}