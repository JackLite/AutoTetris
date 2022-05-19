using System.Collections.Generic;
using Core.AI;
using UnityEngine;

namespace Core.Figures.FigureAlgorithms.FigureT
{
    public class FigureTAlgorithm : FigureAlgorithm
    {
        public FigureTAlgorithm()
        {
            RotatedFigures.Add(FigureRotation.Zero, new FigureTRotationZero());
            RotatedFigures.Add(FigureRotation.ClockWise, new FigureTRotationClockwise());
            RotatedFigures.Add(FigureRotation.CounterClockwise, new FigureTRotationCounterClockwise());
            RotatedFigures.Add(FigureRotation.Mirror, new FigureTRotationMirror());
            
            FigureRotations.Add(FigureRotation.Zero);
            FigureRotations.Add(FigureRotation.Mirror);
            FigureRotations.Add(FigureRotation.ClockWise);
            FigureRotations.Add(FigureRotation.CounterClockwise);
        }
        public override IEnumerable<AiDecision> GetStartDecision(in Vector2Int gridSize)
        {
            var first = new AiDecision
            {
                Column = 0, Row = 0, Rotation = FigureRotation.Mirror, Direction = Direction.Left
            };

            var second = new AiDecision
            {
                Column = gridSize.y - 3, Row = 0, Rotation = FigureRotation.Mirror, Direction = Direction.Right
            };

            var column = Random.Range(0, 1f) > .5f ? 3 : 4;
            var third = new AiDecision
            {
                Column = column, Row = 0, Rotation = FigureRotation.Mirror, Direction = Direction.Bottom
            };
            return new [] { first, second, third };
        }
    }
}