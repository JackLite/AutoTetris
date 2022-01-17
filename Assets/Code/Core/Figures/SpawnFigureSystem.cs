using System;
using System.Collections.Generic;
using System.Linq;
using Core.Figures.FigureAlgorithms;
using Core.GameOver;
using Core.Grid;
using Core.Pause.Signals;
using EcsCore;
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
        private readonly Random _random;
        private readonly Stack<FigureType> _figureBag = new Stack<FigureType>();

        public SpawnFigureSystem()
        {
            _random = new Random();
        }

        public void Init()
        {
            _eventTable.AddEvent<FigureSpawnSignal>();
            FillBag(_figureBag);
        }

        public void Run()
        {
            if (!_eventTable.Has<FigureSpawnSignal>())
                return;

            if (_coreState.IsPaused && !_eventTable.Has<UnpauseSignal>())
                return;

            if (GridService.IsFillSomeAtTopRow(_gridData.FillMatrix))
                return;
            CreateFigure();
        }

        private async void CreateFigure()
        {
            var type = _figureBag.Pop();

            if (_figureBag.Count == 0)
                FillBag(_figureBag);

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
                Type = type, Mono = mono, Row = startRow, Column = startColumn
            };
            entity.Replace(figure);

            _mainScreen.NextFigure.ShowNext(_figureBag.Peek());
            if (FigureAlgorithmFacade.IsFall(_gridData.FillMatrix, figure))
            {
                _eventTable.AddEvent<GameOverSignal>();
                figure.Mono.Delete();
                entity.Destroy();
            }
        }

        private void FillBag(Stack<FigureType> figureBag)
        {
            var variants = Enum.GetValues(typeof(FigureType)).Cast<FigureType>().ToArray();
            _random.Shuffle(variants);

            foreach (var type in variants)
                figureBag.Push(type);
        }
    }
}