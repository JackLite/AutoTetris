using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcsCore;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace MainMenu
{
    public class MainMenuModule : EcsModule
    {
        private GameObject _startScreen;
        private readonly Dictionary<Type, object> _dependencies = new();

        protected override async Task Setup()
        {
            var handler = Addressables.InstantiateAsync("StartScreen");
            await handler.Task;
            _startScreen = handler.Result;
            _dependencies.Add(typeof(StartScreenMono), _startScreen.GetComponent<StartScreenMono>());
        }
        
        public override Dictionary<Type, object> GetDependencies()
        {
            return _dependencies;
        }

        public override void OnDeactivate()
        {
            Addressables.ReleaseInstance(_startScreen);
        }
    }
}