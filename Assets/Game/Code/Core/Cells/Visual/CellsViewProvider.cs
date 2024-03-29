﻿using System.Collections.Generic;
using UnityEngine.AddressableAssets;

namespace Core.Cells.Visual
{
    /// <summary>
    /// Инкапсулирует логику работы с визуалом клеток
    /// </summary>
    public class CellsViewProvider
    {
        private readonly MainScreenMono _mainScreen;
        private CellMono[,] _cellsArray;
        public CellsViewProvider(MainScreenMono mainScreen)
        {
            _mainScreen = mainScreen;
        }

        public void Init(int rows, int columns)
        {
            _cellsArray = new CellMono[rows, columns];
        }

        public CellMono CreateCell(int row, int column)
        {
            var handle = Addressables.InstantiateAsync("Cell", _mainScreen.grid);
            handle.WaitForCompletion();
            var cellMono = handle.Result.GetComponent<CellMono>();
            cellMono.SetPosition(row, column);
            cellMono.SetEmpty();
            _cellsArray[row, column] = cellMono;
            return cellMono;
        }

        public CellMono GetCell(int row, int column)
        {
            return _cellsArray[row, column];
        }

        public IEnumerable<CellMono> GetLightedCells(Direction direction)
        {
            for (var i = 0; i < _cellsArray.GetLength(0); ++i)
            {
                for (var j = 0; j < _cellsArray.GetLength(1); ++j)
                {
                    var cellMono = _cellsArray[i, j];
                    if (cellMono.IsLightUp && cellMono.Direction == direction)
                        yield return cellMono;
                }
            }
        }
    }
}