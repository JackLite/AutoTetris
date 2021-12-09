using Core.Figures;
using Core.Figures.FigureAlgorithms.Path;
using Core.Grid;
using Unity.Collections;

namespace Core.AI
{
    public struct AiMoveVariant
    {
        public int Row;
        public int Column;
        public int Weight;
        public FigureRotation Rotation;
        public int H;
        public int FR;
        public PathAction[] Path;
    }
}