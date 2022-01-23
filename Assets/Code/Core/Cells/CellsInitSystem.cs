using Core.Cells.Visual;
using Core.Grid;
using EcsCore;
using Leopotam.Ecs;

namespace Core.Cells
{
    [EcsSystem(typeof(CoreModule))]
    public class CellsInitSystem : IEcsInitSystem
    {
        private GridData _grid;
        private EcsWorld _world;
        private CellsViewProvider _cellsViewProvider;

        private int _remainCreate;
        public void Init()
        {
            _cellsViewProvider.Init(_grid.Rows, _grid.Columns);
            for (var row = 0; row < _grid.Rows; row++)
            {
                for (var column = 0; column < _grid.Columns; column++)
                {
                    CreateCell(row, column);
                }
            }
        }

        private void CreateCell(int row, int column)
        {
            var view = _cellsViewProvider.CreateCell(row, column);
            var cell = new Cell
            {
                column = column, row = row, view = view
            };
            _world.NewEntity().Replace(cell);
            if (_grid.FillMatrix[row, column])
                _cellsViewProvider.GetCell(row, column).SetImageActive(true);
        }
    }
}