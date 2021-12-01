﻿using Core.AI;
using Core.Cells;
using Core.Figures.FigureAlgorithms;
using Core.Grid;
using Core.Input;
using Core.Pause;
using EcsCore;
using Global;
using Leopotam.Ecs;
using UnityEngine;

namespace Core.Figures
{
    [EcsSystem(typeof(CoreModule))]
    public class MoveFigureSystem : IEcsInitSystem, IEcsRunSystem, IEcsDestroySystem
    {
        private const float START_DELAY = .5f;
        private const float SPEED_VELOCITY = .005f;
        private float _fallCounter;
        private float _currentSpeed;
        private EcsFilter<Figure> _filter;
        private EcsFilter<AiDecision> _decisionsFilter;
        private EcsFilter<Cell> _cells;
        private MainScreenMono _screenMono;
        private GridData _grid;
        private CoreState _coreState;
        private PlayerData _playedData;
        private EcsWorld _world;
        private InputSignal _inputSignal;

        public void Init()
        {
            EcsWorldEventsBlackboard.AddEventHandler<InputSignal>(SaveInputSignal);
            _currentSpeed = START_DELAY;
        }

        private void SaveInputSignal(InputSignal signal)
        {
            _inputSignal = signal;
        }

        public void Run()
        {
            if (_filter.GetEntitiesCount() == 0 || _coreState.IsPaused)
                return;

            ref var figure = ref _filter.Get1(0);

            if (figure.Column > 0 && _decisionsFilter.GetEntitiesCount() > 0)
            {
                if (_inputSignal != null)
                {
                    var aiDecision = GetAiDecision(_inputSignal.Direction);
                    figure.Row = aiDecision.Row;
                    figure.Column = aiDecision.Column;
                    figure.Rotation = aiDecision.Rotation;
                    figure.Mono.SetGridPosition(figure.Row, figure.Column);
                    FinishMove(figure);
                    _fallCounter = _currentSpeed;
                    _inputSignal = null;

                    return;
                }
            }

            _fallCounter -= Time.deltaTime;

            if (_fallCounter >= 0)
                return;

            if (figure.Row > 0 && !IsFall(_grid.FillMatrix, figure))
                figure.Row--;
            figure.Mono.SetGridPosition(figure.Row, figure.Column);

            if (IsFall(_grid.FillMatrix, figure))
            {
                FinishMove(figure);
            }

            _fallCounter = _currentSpeed;
        }

        private AiDecision GetAiDecision(in Direction direction)
        {
            foreach (var i in _decisionsFilter)
            {
                ref var aiDecision = ref _decisionsFilter.Get1(i);
                if (aiDecision.Direction != direction)
                    continue;
                
                return aiDecision;
            }
            return AiDecision.Zero;
        }

        private void FinishMove(Figure figure)
        {
            _world.NewEntity().Replace(new CheckLinesSignal());

            FigureAlgorithmFacade.FillGrid(_grid.FillMatrix, figure);

            foreach (var i in _decisionsFilter)
            {
                _decisionsFilter.GetEntity(i).Destroy();
            }
            
            if (GridService.IsFillSomeAtTopRow(_grid.FillMatrix))
            {
                _world.NewEntity().Replace(new GameOverSignal());
                return;
            }
            _playedData.Scores += 1;
            _world.NewEntity().Replace(new FigureSpawnSignal());

            CreateSingleFigures(in figure);

            figure.Mono.Delete();
            _filter.GetEntity(0).Destroy();
            _currentSpeed *= 1 - SPEED_VELOCITY;
        }

        private void CreateSingleFigures(in Figure figure)
        {
            foreach (var i in _cells)
            {
                ref var cell = ref _cells.Get1(i);
                cell.View.LightDown();
                FigureAlgorithmFacade.UpdateFillCell(figure, cell);
            }
        }

        private static bool IsFall(in bool[,] fillMatrix, in Figure figure)
        {
            return FigureAlgorithmFacade.IsFall(fillMatrix, figure);
        }

        public void Destroy()
        {
            EcsWorldEventsBlackboard.RemoveEventHandler<InputSignal>(SaveInputSignal);

            foreach (var i in _filter)
            {
                _filter.GetEntity(i).Destroy();
            }
        }
    }
}