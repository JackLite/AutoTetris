namespace Core.Figures.FigureAlgorithms.Path
{
    public delegate void PathAction(ref Figure figure);
    public static class PathActions
    {
        public static void MoveLeft(ref Figure figure)
        {
            figure.Column--;
        }
        
        public static void MoveRight(ref Figure figure)
        {
            figure.Column++;
        }

        public static void MoveDown(ref Figure figure)
        {
            figure.Row--;
        }
    }
}