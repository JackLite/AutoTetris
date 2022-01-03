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

        public static int CalculateAggregateHeight(bool[,] fillMatrix)
        {
            var rows = fillMatrix.GetLength(0);
            var columns = fillMatrix.GetLength(1);
            var result = 0;
            for (var column = 0; column < columns; ++column)
            {
                for (var row = rows - 1; row >= 0; --row)
                {
                    if (fillMatrix[row, column])
                    {
                        result += row;
                        break;
                    }
                }
            }

            return result;
        }

        public static int CalculateHoles(bool[,] fillMatrix)
        {
            var rows = fillMatrix.GetLength(0);
            var columns = fillMatrix.GetLength(1);
            var result = 0;
            for (var column = 0; column < columns; ++column)
            {
                var startCount = false;
                for (var row = rows - 1; row >= 0; --row)
                {
                    if (fillMatrix[row, column] && !startCount)
                    {
                        startCount = true;
                        continue;
                    }

                    if (!fillMatrix[row, column] && startCount)
                        result++;
                }
            }

            return result;
        }

        public static int CalculateBumpiness(bool[,] fillMatrix)
        {
            var rows = fillMatrix.GetLength(0);
            var columns = fillMatrix.GetLength(1);
            var result = 0;
            var prev = -1;
            for (var column = 0; column < columns; ++column)
            {
                for (var row = rows - 1; row >= 0; --row)
                {
                    if (fillMatrix[row, column])
                    {
                        if (prev < 0)
                        {
                            prev = row;
                            break;
                        }

                        result += Math.Abs(prev - row);
                        prev = row;
                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Рассчитывает новое состояние игрового поля после проверки линий
        /// Это нужно, чтобы корректно вычислять эффективность хода
        /// </summary>
        public static bool[,] CalculateFieldStateAfterCheckLines(bool[,] fillMatrix)
        {
            var newState = new bool[fillMatrix.GetLength(0), fillMatrix.GetLength(1)];
            Array.Copy(fillMatrix, newState, fillMatrix.Length);
            var fullRows = GetFullRowsIndexes(newState);
            if (fullRows.Count == 0)
                return newState;

            foreach (var row in fullRows)
            {
                for (var column = 0; column < fillMatrix.GetLength(1); ++column)
                    newState[row, column] = false;
            }
            
            var topNotEmptyRow = FindTopNotEmptyRow(newState);
            var bottomRow = FindFirstEmptyRowUnder(topNotEmptyRow, newState);
            
            if (topNotEmptyRow == -1 || bottomRow == -1)
                return newState;

            var moveCount = 0;
            for (var row = 0; row < newState.GetLength(0); ++row)
            {
                for (var column = 0; column < newState.GetLength(1); ++column)
                {
                    if (IsEmptyRow(row, newState))
                        moveCount++;

                    newState[row - moveCount, column] = newState[row, column];
                }
            }

            return newState;
        }

        private static bool IsEmptyRow(int row, bool[,] fillMatrix)
        {
            var columns = fillMatrix.GetLength(1);

            for (var column = 0; column < columns; column++)
            {
                if (fillMatrix[row, column])
                    return false;
            }

            return true;
        }

        public static int FindTopNotEmptyRow(bool[,] fillMatrix)
        {
            var topNotEmptyRow = -1;
            var rows = fillMatrix.GetLength(0);
            for (var row = rows - 1; row > 0; row--)
            {
                if (IsEmptyRow(row, fillMatrix))
                    continue;
                topNotEmptyRow = row;

                break;
            }

            return topNotEmptyRow;
        }


        public static int FindFirstEmptyRowUnder(int underRow, bool[,] fillMatrix)
        {
            for (var row = 0; row < underRow; row++)
            {
                if (!IsEmptyRow(row, fillMatrix))
                    continue;

                return row;
            }

            return -1;
        }
    }
}