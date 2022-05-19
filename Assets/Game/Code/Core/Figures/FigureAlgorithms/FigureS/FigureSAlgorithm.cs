using System.Collections.Generic;
using Core.AI;
using UnityEngine;

namespace Core.Figures.FigureAlgorithms.FigureS
{
    public class FigureSAlgorithm : FigureAlgorithm
    {
        public FigureSAlgorithm()
        {
            RotatedFigures.Add(FigureRotation.Zero, new FigureSRotationZero());
            RotatedFigures.Add(FigureRotation.ClockWise, new FigureSRotationClockwise());
            
            FigureRotations.Add(FigureRotation.Zero);
            FigureRotations.Add(FigureRotation.ClockWise);
        }
        public override IEnumerable<AiDecision> GetStartDecision(in Vector2Int gridSize)
        {
            var first = new AiDecision
            {
                Column = 0, Row = 0, Rotation = FigureRotation.Zero, Direction = Direction.Left
            };

            var second = new AiDecision
            {
                Column = gridSize.y - 2, Row = 0, Rotation = FigureRotation.ClockWise, Direction = Direction.Right
            };

            var third = new AiDecision
            {
                Column = 3, Row = 0, Rotation = FigureRotation.Zero, Direction = Direction.Bottom
            };
            return new [] { first, second, third };
        }
    }
}