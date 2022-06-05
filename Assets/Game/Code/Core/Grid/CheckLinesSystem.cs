using Core.Cells;
using Core.Figures;
using Core.Saving;
using EcsCore;
using Global;
using Global.Audio;
using Global.Saving;
using Global.Settings;
using Global.Settings.Audio;
using Global.Settings.Core;
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
        private CoreSettings _coreSettings;
        private CoreProgressionService _progressionService;

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
            _playerData.currentScores += GetScoresReward(fullRows.Count);
            _eventTable.AddEvent<SaveCoreSignal>();
        }

        private int GetScoresReward(int rowsCount)
        {
            var level = _progressionService.GetLevel(_playerData.currentScores);
            if (rowsCount >= 4)
                return _coreSettings.linesScores[3] * level;

            if (rowsCount > 0)
                return _coreSettings.linesScores[rowsCount - 1];

            return level;
        }
    }
}