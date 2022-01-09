using System;

namespace Core
{
    [Flags]
    public enum Direction
    {
        Top = 1,
        Left = 2,
        Down = 4,
        Right = 8
    }
}