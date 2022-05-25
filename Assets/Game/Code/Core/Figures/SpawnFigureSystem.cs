using System;
using System.Collections.Generic;
using System.Linq;
using Core.Figures.FigureAlgorithms;
using Core.GameOver.Components;
using Core.Grid;
using Core.Pause.Signals;
using EcsCore;
using Global;
using Global.Saving;
using Global.Settings.Core;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Utilities;
using Random = System.Random;

namespace Core.Figures
{
    [EcsSystem(typeof(CoreModule))]
    public class SpawnFigureSystem : IEcsInitSystem, IEcsRunSystem
    {
        private float _counter;
        private MainScreenMono _mainScreen;
        private GridData _gridData;
        private EcsEventTable _eventTable;
        private EcsWorld _world;
        private CoreState _coreState;
        private CoreSettings _settings;
        private StartCoreData _startCoreData;
        private SaveService _saveService;
        private Random _random;
        private Stack<FigureType> _figureBag = new Stack<FigureType>();

        public SpawnFigureSystem()
        {
            _random = new Random();
        }

        public void Init()
        {
            if (_settings.aiEnable)
                _random = new Random(123456789);
            if (_startCoreData.isContinue)
            {
                _figureBag = _saveService.LoadFigureBag();
                if (!_saveService.HasFigure())
                {
                    _eventTable.AddEvent<FigureSpawnSignal>();
                }
                else
                {
                    var figure = _saveService.LoadFigure();
                    CreateFigure(figure.type);
                }
            }
            else
            {
                FillBag(_figureBag);
                _eventTable.AddEvent<FigureSpawnSignal>();
            }
        }

        public void Run()
        {
            if (!_eventTable.Has<FigureSpawnSignal>())
                return;

            if (_coreState.IsPaused && !_eventTable.Has<UnpauseSignal>())
                return;

            if (GridService.IsFillSomeAtTopRow(_gridData.FillMatrix))
                return;

            _saveService.SetHasFigure(true);
            CreateFigure(PopFigureType());
        }

        private async void CreateFigure(FigureType type)
        {
            var name = FiguresUtility.GetFigureAddress(type);
            var task = Addressables.InstantiateAsync(name, Vector2.one * 10000, Quaternion.identity, _mainScreen.grid)
                                   .Task;
            await task;
            var mono = task.Result.GetComponent<FigureMono>();
            var startRow = _gridData.FillMatrix.GetLength(0) - 4;
            var startColumn = _gridData.FillMatrix.GetLength(1) / 2 - 1;
            if (!_saveService.GetTutorCompleted())
                startRow -= 2;
            mono.SetGridPosition(startRow, startColumn);
            mono.Rotate(FigureRotation.Zero);
            var entity = _world.NewEntity();

            var figure = new Figure
            {
                type = type, mono = mono, row = startRow, column = startColumn
            };
            entity.Replace(figure);
            _saveService.SaveCurrentFigure(figure);
            _mainScreen.NextFigure.ShowNext(PeekFigureType());
            if (FigureAlgorithmFacade.IsFall(_gridData.FillMatrix, figure))
            {
                _eventTable.AddEvent<GameOverCoreSignal>();
                _saveService.SetHasFigure(false);
                figure.mono.Delete();
                entity.Destroy();
            }
        }
        private FigureType PopFigureType()
        {
            if (_figureBag.Count == 0)
                FillBag(_figureBag);
            if (_coreState.NextFigure == FigureType.None)
            {
                var figureType = _figureBag.Pop();
                _saveService.SaveFigureBag(_figureBag);
                return figureType;
            }
            var type = _coreState.NextFigure;
            _coreState.NextFigure = FigureType.None;
            return type;
        }

        private FigureType PeekFigureType()
        {
            if (_figureBag.Count == 0)
                FillBag(_figureBag);
            return _figureBag.Peek();
        }

        private void FillBag(Stack<FigureType> figureBag)
        {
            if (!_saveService.GetTutorCompleted())
            {
                figureBag.Push(FigureType.T);
                figureBag.Push(FigureType.I);
                figureBag.Push(FigureType.L);
                return;
            }
            var variants = Enum.GetValues(typeof(FigureType)).Cast<FigureType>().Where(f => f > 0).ToArray();
            _random.Shuffle(variants);

            foreach (var type in variants)
                figureBag.Push(type);
        }
    }
}