using System;
using System.Collections.Generic;
using Core.AI;
using Core.Grid;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.Figures.FigureAlgorithms.FigureO
{
    /// <summary>
    /// [ ] [ ] [ ]
    /// [*] [*] [ ]
    /// [X] [*] [ ]
    /// </summary>
    public class FigureOAlgorithm : FigureAlgorithm, IRotatedFigure
    {
        private readonly GridPosition[] _positions = new GridPosition[4];

        public FigureOAlgorithm()
        {
            RotatedFigures.Add(FigureRotation.Zero, this);

            FigureRotations.Add(FigureRotation.Zero);
        }

        public bool CheckBordersForPlaceFigure(in bool[,] fillMatrix, in GridPosition position)
        {
            var row = position.Row;
            var column = position.Column;

            if (row > fillMatrix.GetLength(0) - 2 || column > fillMatrix.GetLength(1) - 2)
                return false;

            return true;
        }

        bool IRotatedFigure.IsFall(in bool[,] fillMatrix, in Figure figure)
        {
            var rows = fillMatrix.GetLength(0);

            if (figure.row >= rows)
                return false;

            var isFillUnder = fillMatrix[figure.row - 1, figure.column];
            var isFillRightUnder = fillMatrix[figure.row - 1, figure.column + 1];

            return isFillUnder || isFillRightUnder;
        }
        public override IEnumerable<AiDecision> GetStartDecision(in Vector2Int gridSize)
        {
            var first = new AiDecision
            {
                Column = 0, Row = 0, Rotation = FigureRotation.Zero, Direction = Direction.Left
            };

            var second = new AiDecision
            {
                Column = gridSize.y - 2, Row = 0, Rotation = FigureRotation.Zero, Direction = Direction.Right
            };

            var third = new AiDecision
            {
                Column = 4, Row = 0, Rotation = FigureRotation.Zero, Direction = Direction.Bottom
            };
            return new [] { first, second, third };
        }

        public GridPosition[] GetPositions(in GridPosition position)
        {
            _positions[0] = position;
            _positions[1] = position.Right();
            _positions[2] = position.Above();
            _positions[3] = _positions[1].Above();
            return _positions;
        }

        public Direction GetBorderDirectionsForCell(in GridPosition cellPosition, in GridPosition position)
        {
            var positions = GetPositions(position);
            if (cellPosition == positions[0])
                return Direction.Bottom | Direction.Left;
            if (cellPosition == positions[1])
                return Direction.Right | Direction.Bottom;
            if (cellPosition == positions[2])
                return Direction.Top | Direction.Left;
            if (cellPosition == positions[3])
                return Direction.Top | Direction.Right;
            throw new ArgumentException("Wrong position: " + cellPosition);
        }
    }
}