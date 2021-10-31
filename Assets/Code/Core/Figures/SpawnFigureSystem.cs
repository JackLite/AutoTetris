using Core.Grid;
using Core.Moving;
using EcsCore;
using Leopotam.Ecs;
using UnityEngine.AddressableAssets;

namespace Core.Figures
{
    [EcsSystem(typeof(CoreSetup))]
    public class SpawnFigureSystem : IEcsInitSystem, IEcsRunSystem
    {
        private float _counter;
        private MainScreenMono _mainScreen;
        private GridData _gridData;
        private EcsFilter<FigureSpawnSignal> _filter;
        private EcsWorld _world;
            
        public void Init()
        {
            _world.NewEntity().Replace(new FigureSpawnSignal());
        }
            
        public void Run()
        {
            if (_filter.GetEntitiesCount() == 0)
                return;

            CreateFigure();
            _filter.GetEntity(0).Destroy();
        }

        private async void CreateFigure()
        {
            var task = Addressables.InstantiateAsync("Figure_O", _mainScreen.grid).Task;
            await task;
            var mono = task.Result.GetComponent<FigureMono>();
            var startRow = _gridData.FillMatrix.GetLength(0);
            var startColumn = _gridData.FillMatrix.GetLength(1) / 2 - 1;
            mono.SetGridPosition(startRow, startColumn);
            var entity = EcsWorldContainer.world.NewEntity();
            entity.Replace(new FigureComponent
            {
                Type = FigureType.O, Mono = mono, Row = startRow, Column = startColumn
            });
        }

    }
}