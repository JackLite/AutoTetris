using Core.Figures;
using Core.Grid;
using EcsCore;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Core.Cells
{
    [EcsSystem(typeof(CoreModule))]
    public class CellsSystem : IEcsInitSystem, IEcsRunSystem
    {
        private const float FIRST_DELAY = .2f;
        private const float DELAY = .1f;
        private GridData _grid;
        private EcsWorld _world;
        private EcsFilter<Cell> _cells;
        private float _checkSpeed;
        private CellMono[,] _cellsArray;

        public void Init()
        {
            _cellsArray = new CellMono[_grid.Rows, _grid.Columns];

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
            var cell = new Cell
            {
                Column = column, Row = row, State = CellState.Empty, View = cellMono
            };
            _world.NewEntity().Replace(cell);
            cellMono.SetPosition(row, column);
            cellMono.SetImageActive(false);
            _checkSpeed = FIRST_DELAY;
            _cellsArray[row, column] = cellMono;
        }

        public void Run()
        {
            if (!_grid.IsNeedCheckPieces)
                return;

            _checkSpeed -= Time.deltaTime;

            if (_checkSpeed > 0)
                return;

            foreach (var i in _cells)
            {
                ref var cell = ref _cells.Get1(i);

                _cellsArray[cell.Row, cell.Column] = cell.View;
            }

            for (var row = 1; row < _grid.Rows; row++)
            {
                for (var column = 0; column < _grid.Columns; column++)
                {
                    var isFill = _grid.FillMatrix[row, column];

                    if (!isFill)
                        continue;
                    var isFillUnder = _grid.FillMatrix[row - 1, column];

                    if (isFillUnder)
                        continue;
                    _cellsArray[row, column].SetImageActive(false);
                    _grid.FillMatrix[row, column] = false;
                    _cellsArray[row - 1, column].SetImageActive(true);
                    _grid.FillMatrix[row - 1, column] = true;
                }
            }

            _grid.IsNeedCheckPieces = IsNeedCheckPieces();

            _checkSpeed = DELAY;

            if (!_grid.IsNeedCheckPieces)
            {
                _world.NewEntity().Replace(new CheckLinesSignal());
                _checkSpeed = FIRST_DELAY;
            }
        }

        private bool IsNeedCheckPieces()
        {
            for (var row = 1; row < _grid.Rows; row++)
            {
                for (var column = 0; column < _grid.Columns; column++)
                {
                    var isFill = _grid.FillMatrix[row, column];

                    if (!isFill)
                        continue;
                    var isFillUnder = _grid.FillMatrix[row - 1, column];

                    if (!isFillUnder)
                        return true;
                }
            }
            
            return false;
        }
    }
}