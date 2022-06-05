using Core.AI;
using Core.Cells;
using Core.Figures;
using Core.Figures.FigureAlgorithms;
using Core.GameOver.Components;
using Core.Grid;
using Core.Saving;
using EcsCore;
using Global.Saving;
using Global.Settings.Core;
using Leopotam.Ecs;
using UnityEngine;

namespace Core.Moving
{
    [EcsSystem(typeof(CoreModule))]
    public class MoveFigureFinishSystem : IEcsRunSystem
    {
        private float WAIT_TIME = .1f;
        private EcsEventTable _eventTable;
        private GridData _grid;
        private EcsFilter<Figure, FinalFigureComponent> _filter;
        private EcsFilter<Cell> _cells;
        private EcsFilter<AiDecision> _decisionsFilter;
        private bool _isActive;
        private float _processTime;
        private SaveService _saveService;
        private CoreSettings _settings;
        private MainScreenMono _mainScreen;

        public void Run()
        {
            if (_isActive)
                Process();
            if (_filter.GetEntitiesCount() == 0)
                return;

            if (!_isActive)
                StartFinishing();
        }

        private void StartFinishing()
        {
            _isActive = true;
            _processTime = Time.time + WAIT_TIME;
            LightDownMoves();
        }

        private void Process()
        {
            if (_processTime < Time.time || _settings.aiEnable)
            {
                FinishMove();
                _isActive = false;
            }
        }

        private void ClearDecisions()
        {
            foreach (var i in _decisionsFilter)
                _decisionsFilter.GetEntity(i).Destroy();
        }

        private void FinishMove()
        {
            ref var figure = ref _filter.Get1(0);
            _eventTable.AddEvent<CheckLinesSignal>();

            FigureAlgorithmFacade.FillGrid(_grid.FillMatrix, figure);
            _eventTable.AddEvent<SaveCoreSignal>();
            _saveService.Flush();
            CreateSingleFigures(figure);
            ClearDecisions();
            _saveService.SetHasFigure(false);
            figure.mono.Delete();
            _filter.GetEntity(0).Destroy();

            if (GridService.IsFillSomeAtTopRow(_grid.FillMatrix))
            {
                _eventTable.AddEvent<GameOverCoreSignal>();
                return;
            }

            _eventTable.AddEvent<FigureSpawnSignal>();
        }

        private void CreateSingleFigures(in Figure figure)
        {
            foreach (var i in _cells)
            {
                ref var cell = ref _cells.Get1(i);
                if (FigureAlgorithmFacade.IsFigureAtCell(figure, cell))
                {
                    cell.view.LightDown();
                    cell.view.SetImage(figure.mono.CellSprite);
                    cell.figureType = figure.type;
                }
                cell.view.SetImageActive(_grid.FillMatrix[cell.row, cell.column]);
            }
        }

        private void LightDownMoves()
        {
            _mainScreen.ShadowCellsController.HideAll();
            foreach (var i in _cells)
            {
                ref var cell = ref _cells.Get1(i);
                cell.view.LightDown();
                cell.view.SetImageActive(_grid.FillMatrix[cell.row, cell.column]);
            }
        }
    }
}