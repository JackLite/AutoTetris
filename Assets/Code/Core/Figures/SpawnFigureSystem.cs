using System;
using System.Collections.Generic;
using System.Linq;
using Core.Figures.FigureAlgorithms;
using Core.GameOver;
using Core.Grid;
using Core.Pause.Signals;
using EcsCore;
using Global;
using Global.Saving;
using Leopotam.Ecs;
using UnityEngine.AddressableAssets;
using Utilities;

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
        private CoreConfig _coreConfig;
        private SaveService _saveService;
        private readonly Random _random;
        private Stack<FigureType> _figureBag = new Stack<FigureType>();

        public SpawnFigureSystem()
        {
            _random = new Random();
        }

        public void Init()
        {
            if (_coreConfig.isContinue)
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
            var task = Addressables.InstantiateAsync(name, _mainScreen.grid).Task;
            await task;
            var mono = task.Result.GetComponent<FigureMono>();
            var startRow = _gridData.FillMatrix.GetLength(0) - 4;
            var startColumn = _gridData.FillMatrix.GetLength(1) / 2 - 1;
            mono.SetGridPosition(startRow, startColumn);
            var entity = EcsWorldContainer.World.NewEntity();

            var figure = new Figure
            {
                type = type, mono = mono, row = startRow, column = startColumn
            };
            entity.Replace(figure);
            _saveService.SaveCurrentFigure(figure);
            _mainScreen.NextFigure.ShowNext(PeekFigureType());
            if (FigureAlgorithmFacade.IsFall(_gridData.FillMatrix, figure))
            {
                _eventTable.AddEvent<GameOverSignal>();
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
            var variants = Enum.GetValues(typeof(FigureType)).Cast<FigureType>().Where(f => f > 0).ToArray();
            _random.Shuffle(variants);

            foreach (var type in variants)
                figureBag.Push(type);
        }
    }
}