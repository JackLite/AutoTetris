﻿using System.Collections.Generic;
using Core.AI;
using UnityEngine;

namespace Core.Figures.FigureAlgorithms.FigureL
{
    public class FigureLAlgorithm : FigureAlgorithm
    {
        public FigureLAlgorithm()
        {
            RotatedFigures.Add(FigureRotation.Zero, new FigureLRotationZero());
            RotatedFigures.Add(FigureRotation.ClockWise, new FigureLRotationClockwise());
            RotatedFigures.Add(FigureRotation.Mirror, new FigureLRotationMirror());
            RotatedFigures.Add(FigureRotation.CounterClockwise, new FigureLRotationCounterClockwise());
            
            FigureRotations.Add(FigureRotation.Zero);
            FigureRotations.Add(FigureRotation.ClockWise);
            FigureRotations.Add(FigureRotation.Mirror);
            FigureRotations.Add(FigureRotation.CounterClockwise);
        }
        public override IEnumerable<AiDecision> GetStartDecision(in Vector2Int gridSize)
        {
            var first = new AiDecision
            {
                Column = 0, Row = 0, Rotation = FigureRotation.Zero, Direction = Direction.Left
            };

            var second = new AiDecision
            {
                Column = gridSize.y - 3, Row = 0, Rotation = FigureRotation.CounterClockwise, Direction = Direction.Right
            };

            var third = new AiDecision
            {
                Column = 4, Row = 0, Rotation = FigureRotation.CounterClockwise, Direction = Direction.Bottom
            };
            return new [] { first, second, third };
        }
    }
}