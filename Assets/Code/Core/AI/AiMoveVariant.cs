using System.Text;
using Core.Figures;
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

        public int AH;
        public int H;
        public int B;
        

        public override string ToString()
        {
            var stringBuilder = new StringBuilder("Row: ", 11);
            stringBuilder.Append(Row);
            stringBuilder.Append(" Column: ");
            stringBuilder.Append(Column);
            stringBuilder.Append(" Weight: ");
            stringBuilder.Append(Weight);
            stringBuilder.Append(" New Aggregate height: ");
            stringBuilder.Append(AH);
            
            stringBuilder.Append(" New Holes: ");
            stringBuilder.Append(H);
            
            stringBuilder.Append(" New Bumpiness: ");
            stringBuilder.Append(B);
            return stringBuilder.ToString();
        }
    }
}