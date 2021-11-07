using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Core.Grid;
using EcsCore;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Core
{
    public class CoreModule : EcsModule
    {
        private GameObject _mainScreen;
        protected override Type Type => GetType();
        private readonly Dictionary<Type, object> _dependencies;
        public override Type ActivationSignal => typeof(CoreActivationSignal);
        public override Type DeactivationSignal => typeof(CoreActivationSignal);

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
            _dependencies[typeof(GridData)] = new GridData(mainScreen.grid.GetComponent<GridMono>());
        }

        protected override void InsertDependencies(IEcsSystem system)
        {
            var fields = system.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var field in fields)
            {
                var t = field.FieldType;
                if(_dependencies.ContainsKey(t))
                    field.SetValue(system, _dependencies[t]);
            }
        }
    }
}