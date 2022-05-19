using System.Collections.Generic;
using Core.AI;
using UnityEngine;

namespace Core.Figures.FigureAlgorithms.FigureI
{
    public class FigureIAlgorithm : FigureAlgorithm
    {
        public FigureIAlgorithm()
        {
            RotatedFigures.Add(FigureRotation.Zero, new FigureIVertical());
            RotatedFigures.Add(FigureRotation.ClockWise, new FigureIHorizontal());

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
                Column = gridSize.y - 1, Row = 0, Rotation = FigureRotation.Zero, Direction = Direction.Right
            };

            var third = new AiDecision
            {
                Column = 3, Row = 0, Rotation = FigureRotation.ClockWise, Direction = Direction.Bottom
            };

            return new[] { first, second, third };
        }
    }
}