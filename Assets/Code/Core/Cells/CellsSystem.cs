using System;
using Core.Cells.Visual;
using Core.Grid;
using EcsCore;
using Global;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Core.Cells
{
    [EcsSystem(typeof(CoreModule))]
    public class CellsSystem : IEcsInitSystem, IEcsRunSystem, IEcsDestroySystem
    {
        private const float FIRST_DELAY = .2f;
        private const float DELAY = .1f;
        private GridData _grid;
        private MainScreenMono _mainScreen;
        private CoreState _coreState;
        private EcsEventTable _eventTable;
        private EcsWorld _world;
        private EcsFilter<Cell> _cells;
        private float _checkSpeed;
        private CellMono[,] _cellsArray;
        private CoreConfig _coreConfig;
        private GridData _gridData;

        private int _remainCreate;
        public void Init()
        {
            _cellsArray = new CellMono[_grid.Rows, _grid.Columns];
            _checkSpeed = FIRST_DELAY;
            _remainCreate = _grid.Rows * _grid.Columns;
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
            var handle = Addressables.InstantiateAsync("Cell", _mainScreen.grid);
            await handle.Task;
            var cellMono = handle.Result.GetComponent<CellMono>();
            var cell = new Cell
            {
                Column = column, Row = row, State = CellState.Empty, View = cellMono
            };
            _world.NewEntity().Replace(cell);
            cellMono.SetPosition(row, column);
            cellMono.SetEmpty();
            _cellsArray[row, column] = cellMono;
            _remainCreate--;
            if (_remainCreate == 0)
                _eventTable.AddEvent<CellsCreatedSignal>();
        }

        public void Run()
        {
            if (!_grid.IsNeedCheckPieces || _coreState.IsPaused)
                return;

            _checkSpeed -= Time.deltaTime;

            if (_checkSpeed > 0)
                return;

            var topNotEmptyRow = GridService.FindTopNotEmptyRow(_grid.FillMatrix);
            var bottomRow = GridService.FindFirstEmptyRowUnder(topNotEmptyRow, _grid.FillMatrix);

            if (topNotEmptyRow == -1 || bottomRow == -1)
            {
                Finish();

                return;
            }

            MoveDownStartAbove(bottomRow);

            _checkSpeed = DELAY;
            _grid.IsNeedCheckPieces = true;
        }

        private void MoveDownStartAbove(int bottomRow)
        {
            for (var row = bottomRow + 1; row < _grid.Rows; row++)
            {
                for (var column = 0; column < _grid.Columns; column++)
                {
                    var isFill = _grid.FillMatrix[row, column];

                    if (!isFill)
                        continue;

                    var sprite = _cellsArray[row, column].CellSprite;
                    _cellsArray[row, column].SetEmpty();
                    _grid.FillMatrix[row, column] = false;
                    _cellsArray[row - 1, column].SetImage(sprite);
                    _cellsArray[row - 1, column].SetImageActive(true);
                    _grid.FillMatrix[row - 1, column] = true;
                }
            }
        }

        private void Finish()
        {
            _grid.IsNeedCheckPieces = false;
            _eventTable.AddEvent<CheckLinesSignal>();
            _checkSpeed = FIRST_DELAY;
        }

        public void Destroy()
        {
            Array.Clear(_grid.FillMatrix, 0, _grid.FillMatrix.Length);
            foreach (var i in _cells)
            {
                _cells.GetEntity(i).Destroy();
            }
        }
    }
}