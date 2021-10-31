using Core.Cells;
using Core.Figures;
using EcsCore;
using Leopotam.Ecs;
using UnityEngine;

namespace Core.Grid
{
    [EcsSystem(typeof(CoreSetup))]
    public class LinesCheckSystem : IEcsRunSystem
    {
        private EcsFilter<CheckLinesSignal> _signal;
        private EcsFilter<CellComponent> _cells;
        private float _updateTime = 1;
        private GridData _grid;

        public void Run()
        {
            if (_signal.GetEntitiesCount() == 0)
                return;

            for (var row = 0; row < _grid.Rows; row++)
            {
                var rowFull = true;

                for (var column = 0; column < _grid.Columns; column++)
                {
                    if (!_grid.FillMatrix[row, column])
                    {
                        rowFull = false;

                        break;
                    }
                }

                if (!rowFull)
                    continue;
                
                for (var column = 0; column < _grid.Columns; column++)
                {
                    _grid.FillMatrix[row, column] = false;
                }
            }
            
            foreach (var i in _cells)
            {
                ref var cell = ref _cells.Get1(i);
                cell.View.SetImageActive(_grid.FillMatrix[cell.Row, cell.Column]);
            }
            
            _signal.GetEntity(0).Destroy();
        }
    }
}