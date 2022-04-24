using Core.Grid;
using EcsCore;
using EcsCore.DependencyInjection;
using Global;
using Global.Saving;
using Global.Settings.Core;
using Leopotam.Ecs;

namespace Core.Loading
{
    [EcsSystem(typeof(CoreModule))]
    public class LoadingSystem : IEcsPreInitSystem
    {
        private GridData _gridData;
        private StartCoreData startCoreData;
        private SaveService _saveService;
        private CoreSettings _settings;

        [Setup]
        public void Setup(GridData grid, StartCoreData data, SaveService saveService, CoreSettings coreSettings)
        {
            _gridData = grid;
            startCoreData = data;
            _saveService = saveService;
            _settings = coreSettings;
        }
        
        public void PreInit()
        {
            if (!startCoreData.isContinue || _settings.aiEnable)
                return;

            _gridData.FillMatrix = _saveService.LoadFillMatrix(_gridData.Rows, _gridData.Columns);
        }
    }
}