using Core.Cells;
using Core.Figures;
using Core.Saving;
using EcsCore;
using Global;
using Global.Audio;
using Global.Saving;
using Global.Settings;
using Global.Settings.Audio;
using Leopotam.Ecs;

namespace Core.Grid
{
    [EcsSystem(typeof(CoreModule))]
    public class CheckLinesSystem : IEcsRunSystem
    {
        private EcsEventTable _eventTable;
        private EcsFilter<Cell> _cells;
        private GridData _grid;
        private MainScreenMono _mainScreen;
        private PlayerData _playerData;
        private SaveService _saveService;
        private EcsWorld _world;
        private GlobalSettings _settings;

        public void Run()
        {
            if (!_eventTable.Has<CheckLinesSignal>())
                return;

            var fullRows = GridService.GetFullRowsIndexes(_grid.FillMatrix);

            foreach (var row in fullRows)
            {
                for (var column = 0; column < _grid.Columns; column++)
                {
                    _grid.FillMatrix[row, column] = false;
                }
            }

            foreach (var i in _cells)
            {
                ref var cell = ref _cells.Get1(i);
                foreach (var rowIndex in fullRows)
                {
                    if (cell.row != rowIndex)
                        continue;
                    cell.figureType = FigureType.None;
                    cell.view.SetEmpty();
                    cell.view.PlayVfx();
                }
            }
            
            foreach (var rowIndex in fullRows)
            {
                var glow = _mainScreen.GlowEffectPool.Get();
                glow.SetRow(rowIndex);
                glow.Show(v => _mainScreen.GlowEffectPool.Return(v));
            }
            if (fullRows.Count > 0)
            {
                _world.CreateOneFrame().Replace(AudioHelper.Create(_settings, AudioEnum.CoreLineFill));
            }
            _grid.IsNeedCheckPieces = fullRows.Count > 0;
            _grid.IsGridStable = fullRows.Count == 0;
            _playerData.currentScores += fullRows.Count * 10;
            _eventTable.AddEvent<SaveCoreSignal>();
        }
    }
}