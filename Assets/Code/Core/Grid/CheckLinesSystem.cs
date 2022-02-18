using Core.Cells;
using EcsCore;
using Global;
using Global.Saving;
using Leopotam.Ecs;

namespace Core.Grid
{
    [EcsSystem(typeof(CoreModule))]
    public class CheckLinesSystem : IEcsRunSystem
    {
        private EcsEventTable _eventTable;
        private EcsFilter<Cell> _cells;
        private GridData _grid;
        private PlayerData _playerData;
        private SaveService _saveService;

        public void Run()
        {
            if (!_eventTable.Has<CheckLinesSignal>())
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
                foreach (var rowIndex in fullRows)
                {
                    if (cell.row != rowIndex)
                        continue;
                    cell.view.SetEmpty();
                    cell.view.PlayVfx();
                }
            }

            _grid.IsNeedCheckPieces = fullRows.Count > 0;
            _grid.IsGridStable = fullRows.Count == 0;
            _playerData.CurrentScores += fullRows.Count * 10;
            _saveService.SaveFillMatrix(_grid.FillMatrix);
        }
    }
}