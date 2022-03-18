using System;
using System.Collections.Generic;
using Core.Figures;
using Core.Figures.FigureAlgorithms;
using Core.Grid;

namespace Core.Path
{
    public static class Pathfinder
    {
        private static HashSet<GridPosition> _checkedPositions = new HashSet<GridPosition>();
        private static List<GridPosition> _positions = new List<GridPosition>(240);
        private static LinkedList<PathNode> _nodes = new LinkedList<PathNode>();
        private static LinkedList<PathActionData> _actions = new LinkedList<PathActionData>();
        private static int[] _nodeIndexes = new int[128];
        
        public static LinkedList<PathActionData> FindPath(
            in GridPosition from,
            in GridPosition to,
            bool[,] fillMatrix,
            in Figure figure)
        {
            _checkedPositions.Clear();
            _positions.Clear();
            _nodes.Clear();
            _actions.Clear();
            Array.Clear(_nodeIndexes, 0, _nodeIndexes.Length);
            var currentNodeIndex = 0;
            
            var startNode = new PathNode(to);
            _nodes.AddLast(startNode);
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
                    if (_actions.Count > 0)
                        _actions.RemoveLast();
                    continue;
                }

                var position = _nodes.Last.Value.GetVariant(_nodeIndexes[currentNodeIndex]);
                _nodeIndexes[currentNodeIndex]++;
                if (_checkedPositions.Contains(position))
                    continue;
                if (FigureAlgorithmFacade.IsHasSpaceForFigure(fillMatrix, figure, position))
                {
                    _checkedPositions.Add(position);
                    _actions.AddLast(ConvertToAction(_nodes.Last.Value.Place, position));
                    _nodes.AddLast(new PathNode(position));
                    currentNodeIndex++;
                    if (position == from)
                        break;
                }
            }

            if (_nodes.Count > 0)
            {
                foreach (var node in _nodes)
                {
                    _positions.Add(node.Place);
                }
            }
            // если позиция не занята - добавляем в список проверенных позиций и обновляем текущую

            return _actions;
        }

        private static PathActionData ConvertToAction(in GridPosition from, in GridPosition to)
        {
            if (from.Column > to.Column)
                return new PathActionData(PathActions.MoveRight, Direction.Right);
            if (from.Column < to.Column)
                return new PathActionData(PathActions.MoveLeft, Direction.Left);
            if (from.Row < to.Row)
                return new PathActionData(PathActions.MoveDown, Direction.Bottom);

            throw new ArgumentException("There is no action for move from " + from + " to " + to);
        }
    }
}