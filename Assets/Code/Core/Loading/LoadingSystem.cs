using Core.Grid;
using EcsCore;
using EcsCore.DependencyInjection;
using Global;
using Global.Saving;
using Leopotam.Ecs;

namespace Core.Loading
{
    [EcsSystem(typeof(CoreModule))]
    public class LoadingSystem : IEcsPreInitSystem
    {
        private GridData _gridData;
        private StartCoreData startCoreData;
        private SaveService _saveService;

        [Setup]
        public void Setup(GridData grid, StartCoreData data, SaveService saveService)
        {
            _gridData = grid;
            startCoreData = data;
            _saveService = saveService;
        }
        
        public void PreInit()
        {
            if (!startCoreData.isContinue)
                return;

            _gridData.FillMatrix = _saveService.LoadFillMatrix(_gridData.Rows, _gridData.Columns);
        }
    }
}