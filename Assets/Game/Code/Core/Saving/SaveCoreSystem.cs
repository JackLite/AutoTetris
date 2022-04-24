using Core.Cells;
using Core.Figures;
using Core.Grid;
using EcsCore;
using Global.Saving;
using Leopotam.Ecs;

namespace Core.Saving
{
    [EcsSystem(typeof(CoreModule))]
    public class SaveCoreSystem : IEcsRunSystem
    {
        private EcsEventTable _eventTable;
        private EcsFilter<Cell> _cells;
        private SaveService _saveService;
        private GridData _grid;

        public void Run()
        {
            if (!_eventTable.Has<SaveCoreSignal>())
                return;

            var types = new FigureType[_grid.Rows, _grid.Columns];
            foreach (var i in _cells)
            {
                ref var cell = ref _cells.Get1(i);
                types[cell.row, cell.column] = cell.figureType;
            }
            _saveService.SaveCells(types);
            _saveService.SaveFillMatrix(_grid.FillMatrix);
            _saveService.Flush();
        }
    }
}