using System.Collections;
using System.Collections.Generic;
using Core.Figures;
using Core.Grid;
using EcsCore;
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
        private CoreState _coreState;
        private EcsEventTable _eventTable;
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
            if (!_grid.IsNeedCheckPieces || _coreState.IsPaused)
                return;

            _checkSpeed -= Time.deltaTime;

            if (_checkSpeed > 0)
                return;

            var topNotEmptyRow = GetTopNotEmptyRow();
            var bottomRow = GetBottomEmptyRow(topNotEmptyRow);

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

                    _cellsArray[row, column].SetImageActive(false);
                    _grid.FillMatrix[row, column] = false;
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

        private int GetBottomEmptyRow(int topNotEmptyRow)
        {
            for (var row = 0; row < topNotEmptyRow; row++)
            {
                var isEmptyRow = true;

                for (var column = 0; column < _grid.Columns; column++)
                {
                    if (!_grid.FillMatrix[row, column])
                        continue;
                    isEmptyRow = false;

                    break;
                }

                if (!isEmptyRow)
                    continue;

                return row;
            }

            return -1;
        }

        private int GetTopNotEmptyRow()
        {
            var topNotEmptyRow = -1;

            for (var row = _grid.Rows - 1; row > 0; row--)
            {
                var isEmptyRow = true;

                for (var column = 0; column < _grid.Columns; column++)
                {
                    if (!_grid.FillMatrix[row, column])
                        continue;
                    isEmptyRow = false;

                    break;
                }

                if (isEmptyRow)
                    continue;
                topNotEmptyRow = row;

                break;
            }

            return topNotEmptyRow;
        }

        public void Destroy()
        {
            foreach (var i in _cells)
            {
                _cells.GetEntity(i).Destroy();
            }
        }
    }
}