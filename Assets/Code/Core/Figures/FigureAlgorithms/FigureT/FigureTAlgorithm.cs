namespace Core.Figures.FigureAlgorithms.FigureT
{
    public class FigureTAlgorithm : FigureAlgorithm
    {
        public FigureTAlgorithm()
        {
            RotatedFigures.Add(FigureRotation.Zero, new FigureTRotationZero());
            RotatedFigures.Add(FigureRotation.ClockWise, new FigureTRotationClockwise());
            RotatedFigures.Add(FigureRotation.CounterClockwise, new FigureTRotationCounterClockwise());
            RotatedFigures.Add(FigureRotation.Mirror, new FigureTRotationMirror());
            
            FigureRotations.Add(FigureRotation.Zero);
            FigureRotations.Add(FigureRotation.Mirror);
            FigureRotations.Add(FigureRotation.ClockWise);
            FigureRotations.Add(FigureRotation.CounterClockwise);
        }
    }
}