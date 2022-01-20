using Core.AI;
using Core.Cells;
using Core.Figures;
using Core.Figures.FigureAlgorithms;
using Core.GameOver;
using Core.Grid;
using EcsCore;
using Global;
using Global.Saving;
using Leopotam.Ecs;
using UnityEngine;

namespace Core.Moving
{
    [EcsSystem(typeof(CoreModule))]
    public class MoveFigureFinishSystem : IEcsRunSystem
    {
        private const float WAIT_TIME = .2f;
        private EcsEventTable _eventTable;
        private PlayerData _playerData;
        private GridData _grid;
        private EcsFilter<Figure, FinalFigureComponent> _filter;
        private EcsFilter<Cell> _cells;
        private EcsFilter<AiDecision> _decisionsFilter;
        private bool _isActive;
        private float _processTime;
        private SaveService _saveService;

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
            _playerData.CurrentScores += 1;
            LightDownMoves();
        }

        private void Process()
        {
            if (_processTime < Time.time)
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
            _saveService.SaveFillMatrix(_grid.FillMatrix);
            _saveService.Flush();
            CreateSingleFigures(figure);
            ClearDecisions();
            figure.Mono.Delete();
            _filter.GetEntity(0).Destroy();

            if (GridService.IsFillSomeAtTopRow(_grid.FillMatrix))
            {
                _eventTable.AddEvent<GameOverSignal>();
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
                    cell.View.SetImage(figure.Mono.CellSprite);
                cell.View.SetImageActive(_grid.FillMatrix[cell.Row, cell.Column]);
            }
        }

        private void LightDownMoves()
        {
            foreach (var i in _cells)
            {
                ref var cell = ref _cells.Get1(i);
                cell.View.LightDown();
                cell.View.SetImageActive(_grid.FillMatrix[cell.Row, cell.Column]);
            }
        }
    }
}