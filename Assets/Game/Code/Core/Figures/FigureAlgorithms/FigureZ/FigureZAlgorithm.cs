namespace Core.Figures.FigureAlgorithms.FigureZ
{
    public class FigureZAlgorithm : FigureAlgorithm
    {
        public FigureZAlgorithm()
        {
            RotatedFigures.Add(FigureRotation.Zero, new FigureZRotationZero());
            RotatedFigures.Add(FigureRotation.ClockWise, new FigureZRotationClockwise());
            
            FigureRotations.Add(FigureRotation.Zero);
            FigureRotations.Add(FigureRotation.ClockWise);
        }
    }
}