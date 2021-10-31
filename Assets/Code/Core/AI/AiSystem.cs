using System;
using System.Collections.Generic;
using System.Linq;
using Core.Figures;
using Core.Grid;
using EcsCore;
using Leopotam.Ecs;
using UnityEngine;

namespace Core.AI
{
    [EcsSystem(typeof(CoreSetup))]
    public class AiSystem : IEcsRunSystem
    {
        private EcsFilter<FigureComponent>.Exclude<AiDecision> _filter;
        private GridData _gridData;
        private float _timer;

        public void Run()
        {
            if (_filter.GetEntitiesCount() == 0)
            {
                _timer = .5f;
                return;
            }

            if (_timer > 0)
            {
                _timer -= Time.deltaTime;

                return;
            }
            ref var figure = ref _filter.Get1(0);

            var placeToFall = FindBetterPlaceToFall(_gridData.FillMatrix);
            _filter.GetEntity(0)
                   .Replace(new AiDecision
                   {
                       Column = placeToFall.x,
                       Row = placeToFall.y
                   });
            _gridData.Mono.LightUp(placeToFall);
        }

        private static Vector2Int FindBetterPlaceToFall(in bool[,] fillMatrix)
        {
            var comparer = new AiMoveVariantComparer();
            var rows = fillMatrix.GetLength(0);
            var columns = fillMatrix.GetLength(1);
            var variants = new List<AiMoveVariant>();

            // анализируем все столбцы и строки
            for (var row = rows - 1; row >= 0; row--)
            {
                for (var column = 0; column < columns - 1; column++)
                {
                    if (!GridService.IsCanPlaceFigure(fillMatrix, row, column))
                        continue;

                    var variant = new AiMoveVariant
                    {
                        Column = column, Row = row, Weight = 10 * (rows - row) + Math.Abs(column - columns / 2)
                    };

                    variants.Add(variant);
                    variants.Sort(comparer);
                }
            }

            if (variants.Count == 0)
                return Vector2Int.zero;

            var result = variants.Last();

            return new Vector2Int(result.Column, result.Row);
        }
    }
}