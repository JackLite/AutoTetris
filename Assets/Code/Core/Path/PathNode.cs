using System;
using System.Collections.Generic;
using Core.Grid;

namespace Core.Figures.FigureAlgorithms.Path
{
    public struct PathNode
    {
        private readonly Stack<Direction> _directions;
        private readonly GridPosition _place;

        public GridPosition Place => _place;
        
        public PathNode(GridPosition place)
        {
            _directions = new Stack<Direction>(3);
            _directions.Push(Direction.Right);
            _directions.Push(Direction.Left);
            _directions.Push(Direction.Top);
            _place = place;
        }

        public GridPosition PopNextVariant()
        {
            if (_directions.Count == 0)
                throw new Exception("There is not left directions");

            var direction = _directions.Pop();
            return _place.To(direction);
        }

        public bool IsSomeDirectionsLeft()
        {
            return _directions.Count > 0;
        }
    }
}