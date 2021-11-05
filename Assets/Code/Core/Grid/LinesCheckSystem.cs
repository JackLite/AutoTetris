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
        private EcsFilter<Cell> _cells;
        private GridData _grid;

        public void Run()
        {
            if (_signal.GetEntitiesCount() == 0)
                return;

            var fullRows = GridService.GetFullRowsIndexes(_grid.FillMatrix);

            foreach (var row in fullRows)
            {
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
            _grid.IsNeedCheckPieces = fullRows.Count > 0;
        }
    }
}