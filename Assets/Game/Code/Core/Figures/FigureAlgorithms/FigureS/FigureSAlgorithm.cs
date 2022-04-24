namespace Core.Figures.FigureAlgorithms.FigureS
{
    public class FigureSAlgorithm : FigureAlgorithm
    {
        public FigureSAlgorithm()
        {
            RotatedFigures.Add(FigureRotation.Zero, new FigureSRotationZero());
            RotatedFigures.Add(FigureRotation.ClockWise, new FigureSRotationClockwise());
            
            FigureRotations.Add(FigureRotation.Zero);
            FigureRotations.Add(FigureRotation.ClockWise);
        }
    }
}