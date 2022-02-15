using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Cells;
using Core.Cells.Visual;
using Core.Grid;
using Core.Moving;
using EcsCore;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Core
{
    public class CoreModule : EcsModule
    {
        private GameObject _mainScreen;
        private readonly Dictionary<Type, object> _dependencies;

        public CoreModule()
        {
            _dependencies = new Dictionary<Type, object>();
        }
        
        protected override async Task Setup()
        {
            var task = Addressables.InstantiateAsync("MainScreen").Task;
            await task;
            _mainScreen = task.Result;
            var mainScreen = _mainScreen.GetComponent<MainScreenMono>();
            _dependencies[typeof(MainScreenMono)] = mainScreen;
            _dependencies[typeof(GridData)] = new GridData();
            _dependencies[typeof(CoreState)] = new CoreState();
            _dependencies[typeof(MovingData)] = new MovingData();
            _dependencies[typeof(CellsViewProvider)] = new CellsViewProvider(mainScreen);
        }

        protected override Dictionary<Type, object> GetDependencies()
        {
            return _dependencies;
        }

        public override void Deactivate()
        {
            Addressables.ReleaseInstance(_mainScreen);
            base.Deactivate();
        }
    }
}