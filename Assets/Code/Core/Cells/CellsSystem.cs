using System;
using Core.Cells.Visual;
using Core.Grid;
using EcsCore;
using Leopotam.Ecs;
using UnityEngine;

namespace Core.Cells
{
    [EcsSystem(typeof(CoreModule))]
    public class CellsSystem : IEcsRunSystem, IEcsDestroySystem
    {
        private const float FIRST_DELAY = .002f;
        private const float DELAY = .001f;
        private EcsEventTable _eventTable;
        private EcsWorld _world;
        private EcsFilter<Cell> _cells;
        
        private GridData _grid;
        private CoreState _coreState;
        private CellsViewProvider _cellsViewProvider;
        
        private float _checkSpeed;

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

                    var cell = _cellsViewProvider.GetCell(row, column);
                    var cellUnder = _cellsViewProvider.GetCell(row - 1, column);
                    var sprite = cell.CellSprite;
                    cell.SetEmpty();
                    _grid.FillMatrix[row, column] = false;
                    cellUnder.SetImage(sprite);
                    cellUnder.SetImageActive(true);
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