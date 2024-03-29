﻿using Core.Figures;

namespace Core.Path
{
    public delegate void PathAction(ref Figure figure);
    public static class PathActions
    {
        public static void MoveLeft(ref Figure figure)
        {
            figure.column--;
        }
        
        public static void MoveRight(ref Figure figure)
        {
            figure.column++;
        }

        public static void MoveDown(ref Figure figure)
        {
            figure.row--;
        }

        public static void RotateClockwise(ref Figure figure)
        {
            figure.rotation = FigureRotation.ClockWise;
        }
        
        public static void RotateCounterClockwise(ref Figure figure)
        {
            figure.rotation = FigureRotation.CounterClockwise;
        }
        
        public static void RotateMirror(ref Figure figure)
        {
            figure.rotation = FigureRotation.Mirror;
        }
    }
}