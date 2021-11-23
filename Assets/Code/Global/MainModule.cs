﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcsCore;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Global
{
    [EcsGlobalModule]
    public class MainModule : EcsModule
    {
        private GameObject _startScreen;
        private readonly Dictionary<Type, object> _dependencies;

        public MainModule()
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

        protected override Dictionary<Type, object> GetDependencies()
        {
            return _dependencies;
        }
    }
}