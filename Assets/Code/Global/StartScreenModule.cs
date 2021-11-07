using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using EcsCore;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Global
{
    [EcsGlobalModule]
    public class StartScreenModule : EcsModule
    {
        private GameObject _startScreen;
        private readonly Dictionary<Type, object> _dependencies;

        protected override Type Type => GetType();
        public override Type ActivationSignal { get; }
        public override Type DeactivationSignal { get; }
        

        public StartScreenModule()
        {
            _dependencies = new Dictionary<Type, object>();
        }

        protected override async Task Setup()
        {
            var handler = Addressables.InstantiateAsync("StartScreen");
            await handler.Task;
            _startScreen = handler.Result;
            _dependencies.Add(typeof(StartScreenMono), _startScreen.GetComponent<StartScreenMono>());
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