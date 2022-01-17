﻿using System.Collections.Generic;
using System.Linq;
using Core.AI;
using Core.Cells;
using Core.Figures;
using Core.Figures.FigureAlgorithms;
using Core.GameOver;
using Core.Grid;
using Core.Input;
using Core.Path;
using EcsCore;
using Global;
using Leopotam.Ecs;
using UnityEngine;

namespace Core.Moving
{
    [EcsSystem(typeof(CoreModule))]
    public class MoveFigureSystem : IEcsInitSystem, IEcsRunSystem, IEcsDestroySystem
    {
        private float _fallCounter;
        private EcsFilter<Figure>.Exclude<FigureFinishComponent> _activeFigureFilter;
        private EcsFilter<Figure, FigureFinishComponent> _finishFigureFilter;
        private EcsFilter<AiDecision> _decisionsFilter;
        private EcsFilter<Cell> _cells;
        private MainScreenMono _screenMono;
        private GridData _grid;
        private CoreState _coreState;
        private PlayerData _playedData;
        private EcsEventTable _eventTable;
        private EcsWorld _world;
        private InputSignal _inputSignal;
        private MovingData _movingData;

        public void Init()
        {
            EcsWorldEventsBlackboard.AddEventHandler<InputSignal>(SaveInputSignal);
        }

        private void SaveInputSignal(InputSignal signal)
        {
            if (_activeFigureFilter.GetEntitiesCount() > 0)
                _inputSignal = signal;
        }

        public void Run()
        {
            if (_coreState.IsPaused)
                return;

            if (_finishFigureFilter.GetEntitiesCount() != 0)
                ProcessFinishing();
            else if (_activeFigureFilter.GetEntitiesCount() != 0)
                ProcessMoving();
        }

        private void ProcessFinishing()
        {
            ref var figure = ref _finishFigureFilter.Get1(0);
            ref var figureFinish = ref _finishFigureFilter.Get2(0);

            _fallCounter -= Time.deltaTime;

            if (_fallCounter >= 0)
                return;

            if (figureFinish.Actions.Count == 0)
            {
                FinishMove(figure);
                _fallCounter = CalculateFallSpeed(_movingData.currentFallSpeed);
                return;
            }

            if (figureFinish.Actions.All(a => a == PathActions.MoveDown))
                _fallCounter = CalculateFallSpeed(_movingData.finishMoveSpeed);
            else
                _fallCounter = CalculateFallSpeed(_movingData.manipulationSpeed);

            var nextAction = figureFinish.Actions.Pop();
            nextAction.Invoke(ref figure);
            figure.Mono.Rotate(figure.Rotation);
            figure.Mono.SetGridPosition(figure.Row, figure.Column);
        }

        private void ProcessMoving()
        {
            ref var figure = ref _activeFigureFilter.Get1(0);

            if (IsFall(_grid.FillMatrix, figure))
                return;

            if (figure.Column > 0 && _decisionsFilter.GetEntitiesCount() > 0)
            {
                if (_inputSignal != null)
                {
                    var aiDecision = GetAiDecision(_inputSignal.Direction);
                    figure.Rotation = aiDecision.Rotation;
                    var path = Pathfinder.FindPath(figure.Position, aiDecision.Position, _grid.FillMatrix, figure);
                    var finishComponent = new FigureFinishComponent { Actions = new Stack<PathAction>(path) };
                    _activeFigureFilter.GetEntity(0).Replace(finishComponent);
                    _fallCounter = CalculateFallSpeed(_movingData.manipulationSpeed);
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
                FinishMove(figure);
            else
                ClearDecisions();

            _fallCounter = CalculateFallSpeed(_movingData.currentFallSpeed);
        }

        private void ClearDecisions()
        {
            foreach (var i in _decisionsFilter)
                _decisionsFilter.GetEntity(i).Destroy();
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
            _playedData.Scores += 1;
            _eventTable.AddEvent<CheckLinesSignal>();

            FigureAlgorithmFacade.FillGrid(_grid.FillMatrix, figure);

            ClearDecisions();

            CreateSingleFigures(figure);

            figure.Mono.Delete();
            if (_activeFigureFilter.GetEntitiesCount() != 0)
                _activeFigureFilter.GetEntity(0).Destroy();
            if (_finishFigureFilter.GetEntitiesCount() != 0)
                _finishFigureFilter.GetEntity(0).Destroy();

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
                cell.View.LightDown();
                if (FigureAlgorithmFacade.IsFigureAtCell(figure, cell))
                    cell.View.SetImage(figure.Mono.CellSprite);
                cell.View.SetImageActive(_grid.FillMatrix[cell.Row, cell.Column]);
            }
        }

        private static bool IsFall(in bool[,] fillMatrix, in Figure figure)
        {
            return FigureAlgorithmFacade.IsFall(fillMatrix, figure);
        }

        private static float CalculateFallSpeed(float speed)
        {
            return 1f / speed;
        }

        public void Destroy()
        {
            EcsWorldEventsBlackboard.RemoveEventHandler<InputSignal>(SaveInputSignal);

            foreach (var i in _activeFigureFilter)
            {
                _activeFigureFilter.GetEntity(i).Destroy();
            }
        }
    }
}