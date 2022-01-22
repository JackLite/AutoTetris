using System.Collections.Generic;
using System.Linq;
using Core.AI;
using Core.Cells;
using Core.Figures;
using Core.Figures.FigureAlgorithms;
using Core.Grid;
using Core.Input;
using Core.Path;
using EcsCore;
using Global;
using Global.Saving;
using Leopotam.Ecs;
using UnityEngine;

namespace Core.Moving
{
    [EcsSystem(typeof(CoreModule))]
    public class MoveFigureSystem : IEcsInitSystem, IEcsRunSystem, IEcsDestroySystem
    {
        private float _fallCounter;
        private EcsFilter<Figure>.Exclude<FigureMoveChosen> _activeFigureFilter;
        private EcsFilter<Figure, FigureMoveChosen> _finishFigureFilter;
        private EcsFilter<Figure>.Exclude<FinalFigureComponent> _notFinalFigureFilter;
        private EcsFilter<AiDecision> _decisionsFilter;
        private EcsFilter<Cell> _cells;
        private MainScreenMono _screenMono;
        private GridData _grid;
        private CoreState _coreState;
        private PlayerData _playedData;
        private EcsEventTable _eventTable;
        private EcsWorld _world;
        private InputEvent _inputEvent;
        private MovingData _movingData;
        private SaveService _saveService;

        public void Init()
        {
            EcsWorldEventsBlackboard.AddEventHandler<InputEvent>(SaveInputSignal);
        }

        private void SaveInputSignal(InputEvent inputEvent)
        {
            if (_activeFigureFilter.GetEntitiesCount() > 0)
                _inputEvent = inputEvent;
        }

        public void Run()
        {
            if (_coreState.IsPaused || _notFinalFigureFilter.GetEntitiesCount() == 0)
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
                FinishMove();
                _fallCounter = CalculateFallSpeed(_movingData.currentFallSpeed);
                return;
            }

            if (figureFinish.Actions.All(a => a == PathActions.MoveDown))
                _fallCounter = CalculateFallSpeed(_movingData.finishMoveSpeed);
            else
                _fallCounter = CalculateFallSpeed(_movingData.manipulationSpeed);

            var nextAction = figureFinish.Actions.Pop();
            nextAction.Invoke(ref figure);
            figure.mono.Rotate(figure.rotation);
            figure.mono.SetGridPosition(figure.row, figure.column);
        }

        private void ProcessMoving()
        {
            ref var figure = ref _activeFigureFilter.Get1(0);

            if (IsFall(_grid.FillMatrix, figure))
                return;

            if (figure.column > 0 && _decisionsFilter.GetEntitiesCount() > 0)
            {
                if (_inputEvent != null)
                {
                    var aiDecision = GetAiDecision(_inputEvent.Direction);
                    if (aiDecision.Direction != Direction.None)
                    {
                        figure.rotation = aiDecision.Rotation;
                        var path = Pathfinder.FindPath(figure.Position, aiDecision.Position, _grid.FillMatrix, figure);
                        var moveChosen = new FigureMoveChosen { Actions = new Stack<PathAction>(path) };
                        _activeFigureFilter.GetEntity(0).Replace(moveChosen);
                        _fallCounter = CalculateFallSpeed(_movingData.manipulationSpeed);
                    }
                    
                    _inputEvent = null;

                    return;
                }
            }

            _fallCounter -= Time.deltaTime;

            if (_fallCounter >= 0)
                return;

            if (figure.row > 0 && !IsFall(_grid.FillMatrix, figure))
                figure.row--;
            figure.mono.SetGridPosition(figure.row, figure.column);

            if (IsFall(_grid.FillMatrix, figure))
                FinishMove();
            else
                _saveService.SaveCurrentFigure(figure);

            ClearDecisions();

            _fallCounter = CalculateFallSpeed(_movingData.currentFallSpeed);
        }
        private void FinishMove()
        {
            _notFinalFigureFilter.GetEntity(0).Replace(new FinalFigureComponent());
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
            EcsWorldEventsBlackboard.RemoveEventHandler<InputEvent>(SaveInputSignal);

            foreach (var i in _activeFigureFilter)
            {
                _activeFigureFilter.GetEntity(i).Destroy();
            }
        }
    }
}