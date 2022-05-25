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
using Global.Audio;
using Global.Saving;
using Global.Settings;
using Global.Settings.Core;
using Leopotam.Ecs;
using Unity.Mathematics;
using UnityEngine;

namespace Core.Moving
{
    [EcsSystem(typeof(CoreModule))]
    public class MoveFigureSystem : IEcsInitSystem, IEcsRunSystem, IEcsDestroySystem
    {
        private float _fallCounter;
        private int _moveDownActionsCount;
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
        private CoreSettings _coreSettings;
        private SaveService _saveService;
        private GlobalSettings _settings;

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

            if (_fallCounter >= 0 && !_coreSettings.aiEnable)
                return;

            if (figureFinish.actions.Count == 0)
            {
                FinishMove();
                if (figureFinish.verticalActionsCount > 2)
                {
                    _screenMono.GridView.PlayFallEffect();
                    _world.CreateOneFrame().Replace(AudioHelper.Create(_settings, AudioEnum.FigureFall));
                }
                _fallCounter = CalculateFallSpeed(_movingData.currentFallSpeed);
                return;
            }

            if (figureFinish.actions.All(a => a == PathActions.MoveDown))
                _fallCounter = CalculateFallSpeed(GetFinishMoveSpeed(figureFinish));
            else
            {
                var speed = math.max(_coreSettings.ManipulationSpeed, _movingData.currentFallSpeed);
                _fallCounter = CalculateFallSpeed(speed);
            }

            var nextAction = figureFinish.actions.Pop();
            nextAction.Invoke(ref figure);
            figure.mono.Rotate(figure.rotation);
            figure.mono.SetGridPosition(figure.row, figure.column);
        }

        private float GetFinishMoveSpeed(in FigureMoveChosen figureFinish)
        {
            if (_moveDownActionsCount == 0)
                _moveDownActionsCount = figureFinish.actions.Count;
            var time = 1 - (float) figureFinish.actions.Count / _moveDownActionsCount;
            var val = _coreSettings.finishMoveVelocity.Evaluate(time);
            return Mathf.Lerp(_coreSettings.finishMoveMinSpeed, _coreSettings.finishMoveMaxSpeed, val);
        }

        private void ProcessMoving()
        {
            ref var figure = ref _activeFigureFilter.Get1(0);

            if (IsFall(_grid.FillMatrix, figure))
                return;

            if (figure.column > 0 && _decisionsFilter.GetEntitiesCount() > 0)
            {
                if (_coreSettings.aiEnable)
                {
                    var aiDecision = _decisionsFilter.Get1(0);
                    figure.rotation = aiDecision.Rotation;
                    figure.row = aiDecision.Row;
                    figure.column = aiDecision.Column;
                    figure.mono.SetGridPosition(figure.row, figure.column);
                    FinishMove();
                    ClearDecisions();
                    return;
                }
                if (_inputEvent != null)
                {
                    var aiDecision = GetAiDecision(_inputEvent.direction);
                    if (aiDecision.Direction != Direction.None)
                    {
                        figure.rotation = aiDecision.Rotation;
                        var path = FigurePathfinder.FindPath(figure.Position,
                            aiDecision.Position,
                            _grid.FillMatrix,
                            figure);
                        var calculateVerticalCount = CalculateVerticalCount(path);
                        Debug.Log("[calculateVerticalCount] " + calculateVerticalCount);
                        var moveChosen = new FigureMoveChosen
                        {
                            actions = new Stack<PathAction>(path.Select(p => p.action)),
                            verticalActionsCount = calculateVerticalCount
                        };
                        _activeFigureFilter.GetEntity(0).Replace(moveChosen);
                        var speed = math.max(_coreSettings.ManipulationSpeed, _movingData.currentFallSpeed);
                        _fallCounter = CalculateFallSpeed(speed);
                    }

                    _inputEvent = null;

                    return;
                }
            }

            if (!_saveService.GetTutorCompleted())
                return;

            _fallCounter -= Time.deltaTime;

            if (_fallCounter >= 0)
                return;

            if (figure.row > 0 && !IsFall(_grid.FillMatrix, figure))
                figure.row--;
            figure.mono.SetGridPosition(figure.row, figure.column);

            if (IsFall(_grid.FillMatrix, figure))
                FinishMove();
            else
            {
                _saveService.SaveCurrentFigure(figure);
                _world.NewEntity().Replace(new MoveFigureSignal());
            }

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

        private int CalculateVerticalCount(LinkedList<PathActionData> path)
        {
            var node = path.Last;
            var result = 0;
            do
            {
                if (node.Value.direction == Direction.Bottom)
                    result++;
                else
                    result = 0;
                node = node.Previous;
            } while (node != null);

            return result;
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