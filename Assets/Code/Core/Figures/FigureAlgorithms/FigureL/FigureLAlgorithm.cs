namespace Core.Figures.FigureAlgorithms.FigureL
{
    public class FigureLAlgorithm : FigureAlgorithm
    {
        public FigureLAlgorithm()
        {
            RotatedFigures.Add(FigureRotation.Zero, new FigureLRotationZero());
            RotatedFigures.Add(FigureRotation.ClockWise, new FigureLRotationClockwise());
            RotatedFigures.Add(FigureRotation.Mirror, new FigureLRotationMirror());
            RotatedFigures.Add(FigureRotation.CounterClockwise, new FigureLRotationCounterClockwise());
            
            FigureRotations.Add(FigureRotation.Zero);
            FigureRotations.Add(FigureRotation.ClockWise);
            FigureRotations.Add(FigureRotation.Mirror);
            FigureRotations.Add(FigureRotation.CounterClockwise);
        }
    }
}