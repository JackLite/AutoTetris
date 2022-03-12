using System;
using Core.Cells.Visual;
using Core.Figures;
using Core.Grid;
using EcsCore;
using Leopotam.Ecs;
using UnityEngine;

namespace Core.Cells
{
    [EcsSystem(typeof(CoreModule))]
    public class CellsSystem : IEcsInitSystem, IEcsRunSystem, IEcsDestroySystem
    {
        private const float FIRST_DELAY = .2f;
        private const float DELAY = .1f;
        private EcsEventTable _eventTable;
        private EcsWorld _world;
        private EcsFilter<Cell> _cells;

        private GridData _grid;
        private CoreState _coreState;
        private CellsViewProvider _cellsViewProvider;

        private float _checkSpeed;
        private int[,] _cellIndices;

        public void Init()
        {
            _cellIndices = new int[_grid.Rows, _grid.Columns];
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
            foreach (var i in _cells)
            {
                ref var cell = ref _cells.Get1(i);
                _cellIndices[cell.row, cell.column] = i;
            }
            for (var row = bottomRow + 1; row < _grid.Rows; row++)
            {
                for (var column = 0; column < _grid.Columns; column++)
                {
                    var isFill = _grid.FillMatrix[row, column];

                    if (!isFill)
                        continue;

                    var cellView = _cellsViewProvider.GetCell(row, column);
                    var cellUnder = _cellsViewProvider.GetCell(row - 1, column);
                    var sprite = cellView.CellSprite;
                    cellView.SetEmpty();
                    _grid.FillMatrix[row, column] = false;
                    cellUnder.SetImage(sprite);
                    cellUnder.SetImageActive(true);
                    _grid.FillMatrix[row - 1, column] = true;
                    ref var cell = ref _cells.Get1(_cellIndices[row, column]);
                    _cells.Get1(_cellIndices[row - 1, column]).figureType = cell.figureType;
                    cell.figureType = FigureType.None;
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