namespace Core.Figures.FigureAlgorithms.FigureJ
{
    public class FigureJAlgorithm : FigureAlgorithm
    {
        public FigureJAlgorithm()
        {
            RotatedFigures.Add(FigureRotation.Zero, new FigureJRotationZero());
            RotatedFigures.Add(FigureRotation.ClockWise, new FigureJRotationClockwise());
            RotatedFigures.Add(FigureRotation.Mirror, new FigureJRotationMirror());
            RotatedFigures.Add(FigureRotation.CounterClockwise, new FigureJRotationCounterClockwise());

            FigureRotations.Add(FigureRotation.Zero);
            FigureRotations.Add(FigureRotation.ClockWise);
            FigureRotations.Add(FigureRotation.Mirror);
            FigureRotations.Add(FigureRotation.CounterClockwise);
        }
    }
}