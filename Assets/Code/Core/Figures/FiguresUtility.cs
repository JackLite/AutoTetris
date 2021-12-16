using System;

namespace Core.Figures
{
    public static class FiguresUtility
    {
        public static string GetFigureAddress(FigureType type)
        {
            return type switch {
                FigureType.I => "Figure_I",
                FigureType.O => "Figure_O",
                FigureType.T => "Figure_T",
                FigureType.L => "Figure_L",
                FigureType.J => "Figure_J",
                FigureType.Z => "Figure_Z",
                FigureType.S => "Figure_S",
                _            => throw new ArgumentOutOfRangeException (nameof(type), type, null)
            };
        }
    }
}