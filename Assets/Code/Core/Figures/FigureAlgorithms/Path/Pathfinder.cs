using System;
using System.Collections.Generic;
using Core.Grid;

namespace Core.Figures.FigureAlgorithms.Path
{
    public static class Pathfinder
    {
        public static List<PathAction> FindPath(
            in GridPosition from,
            in GridPosition to,
            bool[,] fillMatrix,
            in Figure figure)
        {
            var positions = CheckPath(from, to, fillMatrix, figure);
            var currentPosition = to;
            var result = new List<PathAction>();
            foreach (var nextPos in positions)
            {
                if (currentPosition.Column > nextPos.Column)
                    result.Add(PathActions.MoveRight);
                if (currentPosition.Column < nextPos.Column)
                    result.Add(PathActions.MoveLeft);
                if (currentPosition.Row < nextPos.Row)
                    result.Add(PathActions.MoveDown);
                currentPosition = nextPos;
            }
            return result;
            return FindActions(from, to);
        }

        private static List<GridPosition> CheckPath(
            in GridPosition from,
            in GridPosition to,
            bool[,] fillMatrix,
            in Figure figure)
        {
            var currentPosition = to;
            var positions = new List<GridPosition>();
            var startNode = new PathNode(currentPosition);
            var checkedPositions = new List<GridPosition>();
            var nodes = new LinkedList<PathNode>();
            nodes.AddLast(startNode);
            var iterations = 1000;
            // берём следующую позицию рядом
            while (nodes.Count > 0)
            {
                if (iterations < 0)
                    break;
                iterations--;
                if (!nodes.Last.Value.IsSomeDirectionsLeft())
                {
                    nodes.RemoveLast();
                    continue;
                }
                var position = nodes.Last.Value.PopNextVariant();
                if (checkedPositions.Contains(position))
                    continue;
                if (FigureAlgorithmFacade.IsHasSpaceForFigure(fillMatrix, figure, position))
                {
                    checkedPositions.Add(position);
                    nodes.AddLast(new PathNode(position));
                    if (position == from)
                        break;
                }
            }
            if (nodes.Count > 0)
            {
                foreach (var node in nodes)
                {
                    positions.Add(node.Place);
                }
            }
            // если позиция не занята - добавляем в список проверенных позиций и обновляем текущую
            
            return positions;
        }

        private static List<PathAction> FindActions(in GridPosition from, in GridPosition to)
        {
            var currentPosition = to;

            var result = new List<PathAction>();
            var iterations = 100;
            while (currentPosition != from)
            {
                var hDiff = Math.Abs(currentPosition.Column - from.Column);
                var vDiff = from.Row - currentPosition.Row;
                if (hDiff > vDiff)
                {
                    if (currentPosition.Column > from.Column)
                    {
                        currentPosition = currentPosition.Left();
                        result.Add(PathActions.MoveRight);
                    }
                    else if (currentPosition.Column < from.Column)
                    {
                        currentPosition = currentPosition.Right();
                        result.Add(PathActions.MoveLeft);
                    }
                }
                else
                {
                    currentPosition = currentPosition.Above();
                    result.Add(PathActions.MoveDown);
                }
                iterations--;
                if (iterations < 0)
                    break;
            }
            return result;
        }
    }
}