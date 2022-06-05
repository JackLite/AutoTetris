using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.AI;
using Core.Cells.Visual;
using Core.CoreDebug;
using Core.Figures;
using Core.Grid;
using Core.Moving;
using Core.Pause;
using Core.Saving;
using EcsCore;
using Global.Leaderboard.Services;
using Global.Settings.Core;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Core
{
    public class CoreModule : EcsModule
    {
        private GameObject _mainScreen;
        private readonly Dictionary<Type, object> _dependencies;
        private CoreSettingsContainer _coreSettings;

        public CoreModule()
        {
            _dependencies = new Dictionary<Type, object>();
        }

        protected override async Task Setup()
        {
            _mainScreen = await Addressables.InstantiateAsync("MainScreen").Task;
            var mainScreen = _mainScreen.GetComponent<MainScreenMono>();
            _dependencies[typeof(MainScreenMono)] = mainScreen;
            _dependencies[typeof(PauseScreenView)] = mainScreen.PauseScreen;
            _coreSettings = await Addressables.LoadAssetAsync<CoreSettingsContainer>("CoreSettings").Task;
            var gridSize = new Vector2Int(24, 10);
            if (_coreSettings.coreSettings.aiEnable)
                gridSize = new Vector2Int(12, 10);
            _dependencies[typeof(GridData)] = new GridData(gridSize);
            _dependencies[typeof(CoreState)] = new CoreState();
            _dependencies[typeof(MovingData)] = new MovingData();
            _dependencies[typeof(CellsViewProvider)] = new CellsViewProvider(mainScreen);
        }

        public override Dictionary<Type, object> GetDependencies()
        {
            return _dependencies;
        }

        public override void OnDeactivate()
        {
            Addressables.ReleaseInstance(_mainScreen);
            Addressables.Release(_coreSettings);
        }

        protected override Dictionary<Type, int> GetSystemsOrder()
        {
            return new Dictionary<Type, int>
            {
                { typeof(SpawnFigureSystem), -12 },
                { typeof(AiSystem), -10 },
                { typeof(MoveFigureSystem), -9 },
                { typeof(MoveFigureFinishSystem), -8 },
                { typeof(CheckLinesSystem), -7 },
                { typeof(DebugCoreSystem), 10 },
                { typeof(SaveCoreSystem), 10 }
            };
        }
    }
}