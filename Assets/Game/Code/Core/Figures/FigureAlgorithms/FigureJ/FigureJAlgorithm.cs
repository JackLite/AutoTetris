using System.Collections.Generic;
using Core.AI;
using UnityEngine;

namespace Core.Figures.FigureAlgorithms.FigureJ
{
    public class FigureJAlgorithm : FigureAlgorithm
    {
        public FigureJAlgorithm()
        {
            RotatedFigures.Add(FigureRotation.Zero, new FigureJRotationZero());
            RotatedFigures.Add(FigureRotation.ClockWise, new FigureJRotationClockwise());
            RotatedFigures.Add(FigureRotation.Mirror, new FigureJRotationMirror());
            RotatedFigures.Add(FigureRotation.CounterClockwise, new FigureJRotationCounterClockwise());

            FigureRotations.Add(FigureRotation.Zero);
            FigureRotations.Add(FigureRotation.ClockWise);
            FigureRotations.Add(FigureRotation.Mirror);
            FigureRotations.Add(FigureRotation.CounterClockwise);
        }
        public override IEnumerable<AiDecision> GetStartDecision(in Vector2Int gridSize)
        {
            var first = new AiDecision
            {
                Column = 0, Row = 0, Rotation = FigureRotation.ClockWise, Direction = Direction.Left
            };

            var second = new AiDecision
            {
                Column = gridSize.y - 2, Row = 0, Rotation = FigureRotation.Zero, Direction = Direction.Right
            };

            var third = new AiDecision
            {
                Column = 4, Row = 0, Rotation = FigureRotation.ClockWise, Direction = Direction.Bottom
            };
            return new [] { first, second, third };
        }
    }
}