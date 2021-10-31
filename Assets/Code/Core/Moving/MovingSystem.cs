using Core.Figures;
using Core.Grid;
using EcsCore;
using Leopotam.Ecs;
using UnityEngine;

namespace Core.Moving
{
    //[EcsSystem(typeof(CoreSetup))]
    public class MovingSystem : IEcsRunSystem
    {
        private EcsFilter<PositionComponent, MoveComponent> _filter;
        private GridData _grid;
        private EcsWorld _world;
        public void Run()
        {
            foreach (var i in _filter)
            {
                ref var move = ref _filter.Get2(i);

                if (move.CurrentDelay > 0)
                {
                    move.CurrentDelay -= Time.deltaTime;

                    return;
                }
                ref var pos = ref _filter.Get1(i);
                var entity = _filter.GetEntity(i);

                if (IsFall(pos))
                {
                    entity.Replace(new FigureSpawnSignal());
                    return;
                }
                move.CurrentDelay = move.Speed;
                pos.Row--;

                entity.Replace(new PositionChangeSignal());
            }
        }
        
        private bool IsFall(in PositionComponent pos)
        {
            var rows = _grid.Rows;

            if (pos.Row >= rows)
                return false;

            if (pos.Row == 0)
                return true;

            var isFillUnder = _grid.FillMatrix[pos.Row - 1, pos.Column];
            var isFillRightUnder = _grid.FillMatrix[pos.Row, pos.Column + 1];

            return isFillUnder || isFillRightUnder;
        }
    }
}