using System;

namespace Core
{
    [Flags]
    public enum Direction
    {
        None = 0,
        Top = 1,
        Left = 2,
        Bottom = 4,
        Right = 8
    }
}