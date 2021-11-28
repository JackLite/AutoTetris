using Core.Figures;

namespace Core.AI
{
    public struct AiDecision
    {
        public int Row;
        public int Column;
        public FigureRotation Rotation;
        public Direction Direction;
        public static AiDecision Zero { get; } = new AiDecision();
    }
}