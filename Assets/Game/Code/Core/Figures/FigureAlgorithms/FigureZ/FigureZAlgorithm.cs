using System.Collections.Generic;
using Core.AI;
using UnityEngine;

namespace Core.Figures.FigureAlgorithms.FigureZ
{
    public class FigureZAlgorithm : FigureAlgorithm
    {
        public FigureZAlgorithm()
        {
            RotatedFigures.Add(FigureRotation.Zero, new FigureZRotationZero());
            RotatedFigures.Add(FigureRotation.ClockWise, new FigureZRotationClockwise());
            
            FigureRotations.Add(FigureRotation.Zero);
            FigureRotations.Add(FigureRotation.ClockWise);
        }
        public override IEnumerable<AiDecision> GetStartDecision(in Vector2Int gridSize)
        {
            var first = new AiDecision
            {
                Column = 0, Row = 0, Rotation = FigureRotation.ClockWise, Direction = Direction.Left
            };

            var second = new AiDecision
            {
                Column = gridSize.y - 3, Row = 0, Rotation = FigureRotation.Zero, Direction = Direction.Right
            };

            var third = new AiDecision
            {
                Column = 4, Row = 0, Rotation = FigureRotation.Zero, Direction = Direction.Bottom
            };
            return new [] { first, second, third };
        }
    }
}