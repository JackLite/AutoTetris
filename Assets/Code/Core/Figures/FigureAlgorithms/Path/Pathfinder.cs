using System;
using System.Collections.Generic;
using System.Linq;
using Core.Grid;

namespace Core.Figures.FigureAlgorithms.Path
{
    public static class Pathfinder
    {
        public static LinkedList<PathAction> FindPath(
            in GridPosition from,
            in GridPosition to,
            bool[,] fillMatrix,
            in Figure figure)
        {
            return CheckPath(from, to, fillMatrix, figure);;
        }

        private static LinkedList<PathAction> CheckPath(
            in GridPosition from,
            in GridPosition to,
            bool[,] fillMatrix,
            in Figure figure)
        {
            var positions = new List<GridPosition>();
            var startNode = new PathNode(to);
            var checkedPositions = new List<GridPosition>();
            var nodes = new LinkedList<PathNode>();
            var actions = new LinkedList<PathAction>();
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
                    if(actions.Count > 0)
                        actions.RemoveLast();
                    continue;
                }
                var position = nodes.Last.Value.PopNextVariant();
                if (checkedPositions.Contains(position))
                    continue;
                if (FigureAlgorithmFacade.IsHasSpaceForFigure(fillMatrix, figure, position))
                {
                    checkedPositions.Add(position);
                    actions.AddLast(ConvertToAction(nodes.Last.Value.Place, position));
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

            return actions;
        }

        private static PathAction ConvertToAction(in GridPosition from, in GridPosition to)
        {
            if (from.Column > to.Column)
                return PathActions.MoveRight;
            if (from.Column < to.Column)
                return PathActions.MoveLeft;
            if (from.Row < to.Row)
                return PathActions.MoveDown;

            throw new ArgumentException("There is no action for move from " + from + " to " + to);
        }
    }
}