﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.AI;
using Core.Cells.Visual;
using Core.CoreDebug;
using Core.Figures;
using Core.Grid;
using Core.Input;
using Core.Moving;
using Core.Pause;
using Core.Saving;
using Core.Tutorial;
using EcsCore;
using Global.Settings.Core;
using MainMenu;
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
            _dependencies[typeof(MovingData)] = new MovingData { isMoveAllowed = true };
            _dependencies[typeof(CellsViewProvider)] = new CellsViewProvider(mainScreen);
            _dependencies[typeof(CoreProgressionService)] = new CoreProgressionService(_coreSettings.coreSettings);
            var data = new EcsOneData<TutorialProgressData>();
            data.SetData(new TutorialProgressData { delay = 3 });
            OneDataDict[typeof(TutorialProgressData)] = data;

            var swipeData = new SwipeData { state = SwipeState.Finished };
            var swipeEcsData = new EcsOneData<SwipeData>();
            swipeEcsData.SetData(swipeData);
            OneDataDict[typeof(SwipeData)] = swipeEcsData;

            EcsWorldContainer.World.DeactivateModule<MainMenuModule>();
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