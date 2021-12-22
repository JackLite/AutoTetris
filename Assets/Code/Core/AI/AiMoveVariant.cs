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
        public FigureRotation Rotation;

        public float Weight;
    }
}