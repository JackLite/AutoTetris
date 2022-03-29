using System;
using System.Collections.Generic;
using Core.Grid;

namespace Core.Path
{
    public static class CellPathfinder
    {
        private static readonly LinkedList<GridPosition> _pathPositions = new LinkedList<GridPosition>();
        private static readonly LinkedList<PathNode> _nodes = new LinkedList<PathNode>();
        private static readonly int[] _nodeIndexes = new int[128];

        /// <summary>
        /// Строит путь до строки
        /// </summary>
        /// <param name="from"></param>
        /// <param name="toRow"></param>
        /// <param name="fillMatrix"></param>
        /// <param name="seen">Список проверенных позиций</param>
        /// <returns>true/false в зависимости от того, найден ли путь</returns>
        public static bool GetPathToRow(
            in GridPosition from,
            int toRow,
            bool[,] fillMatrix,
            out LinkedList<GridPosition> seen)
        {
            seen = new LinkedList<GridPosition>();
            seen.AddLast(from);
            _nodes.Clear();
            _pathPositions.Clear();
            Array.Clear(_nodeIndexes, 0, _nodeIndexes.Length);
            var currentNodeIndex = 0;
            
            var startNode = new PathNode(from);
            _nodes.AddFirst(startNode);
            var iterations = 1000;
            // берём следующую позицию рядом
            while (_nodes.Count > 0)
            {
                if (iterations < 0)
                    break;
                iterations--;
                if (_nodeIndexes[currentNodeIndex] >= 3)
                {
                    _nodes.RemoveLast();
                    _nodeIndexes[currentNodeIndex] = 0;
                    currentNodeIndex--;
                    continue;
                }

                var position = _nodes.Last.Value.GetVariant(_nodeIndexes[currentNodeIndex]);
                _nodeIndexes[currentNodeIndex]++;
                if (seen.Contains(position))
                    continue;
                if (position.Row >= fillMatrix.GetLength(0) || position.Column >= fillMatrix.GetLength(1))
                    continue;
                if (position.Row < 0 || position.Column < 0)
                    continue;
                if (!fillMatrix[position.Row, position.Column])
                {
                    seen.AddLast(position);
                    _nodes.AddLast(new PathNode(position));
                    currentNodeIndex++;
                    if (position.Row == toRow)
                        return true;
                }
            }

            return false;
        }
    }
}