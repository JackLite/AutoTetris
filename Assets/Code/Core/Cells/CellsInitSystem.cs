using Core.Grid;
using EcsCore;
using Leopotam.Ecs;
using UnityEngine.AddressableAssets;

namespace Core.Cells
{
    [EcsSystem(typeof(CoreSetup))]
    public class CellsInitSystem : IEcsInitSystem
    {
        private GridData _grid;
        private EcsWorld _world;
        public void Init()
        {
            for (var row = 0; row < _grid.Rows; row++)
            {
                for (var column = 0; column < _grid.Columns; column++)
                {
                    CreateCell(row, column);
                }
            }
        }

        private async void CreateCell(int row, int column)
        {
            var handle = Addressables.InstantiateAsync("Cell", _grid.Mono.transform);
            await handle.Task;
            var cellMono = handle.Result.GetComponent<CellMono>();
            var cell = new CellComponent
            {
                Column = column, Row = row, State = CellState.Empty, View = cellMono
            };
            _world.NewEntity().Replace(cell);
            cellMono.SetPosition(row, column);
            cellMono.SetImageActive(false);
        }
    }
}