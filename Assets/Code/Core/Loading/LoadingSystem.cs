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
        private StartCoreSettings _startCoreSettings;
        private SaveService _saveService;

        [Setup]
        public void Setup(GridData grid, StartCoreSettings settings, SaveService saveService)
        {
            _gridData = grid;
            _startCoreSettings = settings;
            _saveService = saveService;
        }
        
        public void PreInit()
        {
            if (!_startCoreSettings.isContinue)
                return;

            _gridData.FillMatrix = _saveService.LoadFillMatrix(_gridData.Rows, _gridData.Columns);
        }
    }
}