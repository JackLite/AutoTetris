using System;
using Core.Grid;
using EcsCore;
using Leopotam.Ecs;
using UnityEngine.AddressableAssets;

namespace Core.Figures
{
    [EcsSystem(typeof(CoreModule))]
    public class SpawnFigureSystem : IEcsInitSystem, IEcsRunSystem
    {
        private float _counter;
        private MainScreenMono _mainScreen;
        private GridData _gridData;
        private EcsFilter<FigureSpawnSignal> _filter;
        private EcsWorld _world;
        private readonly Random _random;

        public SpawnFigureSystem()
        {
            _random = new Random();
        }
        
        public void Init()
        {
            _world.NewEntity().Replace(new FigureSpawnSignal());
        }
            
        public void Run()
        {
            if (_filter.GetEntitiesCount() == 0)
                return;

            if (GridService.IsFillSomeAtTopRow(_gridData.FillMatrix))
                return;
            CreateFigure();
            _filter.GetEntity(0).Destroy();
        }

        private async void CreateFigure()
        {
            var type = (FigureType) _random.Next(0, 3);
            
            var name = GetName(type);
            var task = Addressables.InstantiateAsync(name, _mainScreen.grid).Task;
            await task;
            var mono = task.Result.GetComponent<FigureMono>();
            var startRow = _gridData.FillMatrix.GetLength(0) - 4;
            var startColumn = _gridData.FillMatrix.GetLength(1) / 2 - 1;
            mono.SetGridPosition(startRow, startColumn);
            var entity = EcsWorldStartup.world.NewEntity();

            entity.Replace(new Figure
            {
                Type = type, Mono = mono, Row = startRow, Column = startColumn
            });
        }


        private string GetName(FigureType type)
        {
            switch (type)
            {
                case FigureType.I:
                    return "Figure_I";
                case FigureType.O:
                    return "Figure_O";
                case FigureType.T:
                    return "Figure_T";
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}