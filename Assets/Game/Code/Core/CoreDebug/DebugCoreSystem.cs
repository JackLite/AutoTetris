using Core.Cells;
using Core.Figures;
using Core.Grid;
using Core.Pause.Signals;
using EcsCore;
using Global;
using Leopotam.Ecs;

namespace Core.CoreDebug
{
    [EcsSystem(typeof(CoreModule))]
    public class DebugCoreSystem : IEcsInitSystem
    {
        private EcsEventTable _eventTable;
        private CoreState _coreState;
        private StartCoreData startCoreData;
        private MainScreenMono _mainScreenMono;
        private GridData _gridData;
        private EcsFilter<Cell> _cells;

        public void Init()
        {
            _mainScreenMono.DebugMono.gameObject.SetActive(startCoreData.isDebug);
            if (!startCoreData.isDebug)
                return;

            _eventTable.AddEvent<PauseSignal>();
            _mainScreenMono.SwipeMono.SetActive(false);
            _mainScreenMono.DebugMono.OnStartClick += StartGame;
            _coreState.NextFigure = FigureType.T;
            foreach (var i in _cells)
            {
                ref var cell = ref _cells.Get1(i);
                cell.view.SetButtonState(startCoreData.isDebug);
                var pos = cell.Position;
                cell.view.DebugCellClick += () => OnCellClick(pos);
            }
        }

        private void StartGame()
        {
            foreach (var i in _cells)
            {
                ref var cell = ref _cells.Get1(i);
                var position = cell.Position;
                if (cell.Position != position)
                    continue;
                var isFill = _gridData.FillMatrix[position.Row, position.Column];
                cell.view.ChangeOpacity(1);
                cell.view.SetImageActive(isFill);
            }
            _eventTable.AddEvent<UnpauseSignal>();
            _eventTable.AddEvent<FigureSpawnSignal>();
            _mainScreenMono.DebugMono.gameObject.SetActive(false);
            _mainScreenMono.SwipeMono.SetActive(true);
        }

        private void OnCellClick(in GridPosition position)
        {
            _gridData.FillMatrix[position.Row, position.Column] = !_gridData.FillMatrix[position.Row, position.Column];
            foreach (var i in _cells)
            {
                ref var cell = ref _cells.Get1(i);
                if (cell.Position != position)
                    continue;
                var isFill = _gridData.FillMatrix[position.Row, position.Column];
                cell.view.ChangeOpacity(isFill ? 1 : 0.5f);
                break;
            }
        }
    }
}