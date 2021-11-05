using Core.Figures;
using UnityEngine;

namespace Core.AI
{
    public struct AiDecision
    {
        public int Row;
        public int Column;
        public FigureRotation Rotation;
        public static AiDecision Zero { get; } = new AiDecision();
    }
}