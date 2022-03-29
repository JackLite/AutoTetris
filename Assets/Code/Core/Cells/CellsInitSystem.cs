using System.Collections.Generic;
using Core.Cells.Visual;
using Core.Figures;
using Core.Grid;
using EcsCore;
using Global.Saving;
using Global.Settings.Core;
using JetBrains.Annotations;
using Leopotam.Ecs;
using UnityEngine.AddressableAssets;

namespace Core.Cells
{
    [EcsSystem(typeof(CoreModule))]
    public class CellsInitSystem : IEcsInitSystem
    {
        private CellsViewProvider _cellsViewProvider;
        private CoreSettings _coreSettings;
        private EcsWorld _world;
        private GridData _grid;
        private SaveService _saveService;

        private int _remainCreate;
        private readonly Dictionary<FigureType, AssetReference> _figureTypeToSpriteMap =
            new Dictionary<FigureType, AssetReference>();
        public void Init()
        {
            foreach (var typeToSprite in _coreSettings.figureToSpriteMap)
            {
                _figureTypeToSpriteMap[typeToSprite.figureType] = typeToSprite.sprite;
            }
            _cellsViewProvider.Init(_grid.Rows, _grid.Columns);
            var savedTypes = _saveService.LoadCells(_grid.FillMatrix.GetLength(0), _grid.FillMatrix.GetLength(1));
            for (var row = 0; row < _grid.Rows; row++)
            {
                for (var column = 0; column < _grid.Columns; column++)
                {
                    CreateCell(row, column, savedTypes);
                }
            }
        }

        private void CreateCell(int row, int column, [CanBeNull] FigureType[,] savedTypes)
        {
            var view = _cellsViewProvider.CreateCell(row, column);
            var cell = new Cell
            {
                column = column, row = row, view = view
            };
            
            if (_grid.FillMatrix[row, column])
                _cellsViewProvider.GetCell(row, column).SetImageActive(true);

            if (savedTypes != null && savedTypes.Length > 0 && savedTypes[row, column] != FigureType.None)
            {
                cell.figureType = savedTypes[row, column];
                cell.view.SetImageAsync(_figureTypeToSpriteMap[cell.figureType]);
            }
            _world.NewEntity().Replace(cell);
        }
    }
}