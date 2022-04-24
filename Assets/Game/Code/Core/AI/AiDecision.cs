using Core.Figures;
using Core.Grid;

namespace Core.AI
{
    public struct AiDecision
    {
        public int Row;
        public int Column;
        public FigureRotation Rotation;
        public Direction Direction;
        public GridPosition Position => new GridPosition(Row, Column);
        public static AiDecision Zero { get; } = new AiDecision();
    }
}