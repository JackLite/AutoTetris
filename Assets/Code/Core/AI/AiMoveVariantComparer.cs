using System;
using System.Collections;
using System.Collections.Generic;

namespace Core.AI
{
    public class AiMoveVariantComparer : IComparer<AiMoveVariant>, IComparer
    {
        public int Compare(AiMoveVariant x, AiMoveVariant y)
        {
            return x.Weight.CompareTo(y.Weight);
        }

        public int Compare(object x, object y)
        {
            if (!(x is AiMoveVariant && y is AiMoveVariant))
                throw new ArgumentException();

            return Compare((AiMoveVariant)x, (AiMoveVariant)y);
        }
    }
}