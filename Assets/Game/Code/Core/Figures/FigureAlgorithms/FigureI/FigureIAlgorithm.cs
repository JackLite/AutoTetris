namespace Core.Figures.FigureAlgorithms.FigureI
{
    public class FigureIAlgorithm : FigureAlgorithm
    {
        public FigureIAlgorithm()
        {
            RotatedFigures.Add(FigureRotation.Zero, new FigureIVertical());
            RotatedFigures.Add(FigureRotation.ClockWise, new FigureIHorizontal());

            FigureRotations.Add(FigureRotation.Zero);
            FigureRotations.Add(FigureRotation.ClockWise);
        }
    }
}