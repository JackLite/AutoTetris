using System;
using System.Collections.Generic;
using Core.Grid;

namespace Core.Path
{
    public struct PathNode
    {
        private readonly GridPosition _place;

        public GridPosition Place => _place;
        
        public PathNode(GridPosition place)
        {
            _place = place;
        }

        public GridPosition GetVariant(int index)
        {
            return _place.To(GetDirection(index));
        }

        private Direction GetDirection(int index)
        {
            if (index == 0)
                return Direction.Top;
            if (index == 1)
                return Direction.Left;
            if (index == 2)
                return Direction.Right;
            throw new ArgumentOutOfRangeException("There is not left directions");
        }
    }
}