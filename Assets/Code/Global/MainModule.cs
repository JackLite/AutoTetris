using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcsCore;
using Global.Ads;
using Global.Saving;
using Global.Settings;
using Global.Settings.Core;
using MainMenu;
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
            _dependencies.Add(typeof(PlayerData), new PlayerData());
            _dependencies.Add(typeof(AdsService), new AdsService());
            _dependencies.Add(typeof(StartCoreData), new StartCoreData());
            _dependencies.Add(typeof(SaveService), new SaveService());
            await LoadSettings();
        }
        private async Task LoadSettings()
        {
            var handler = Addressables.LoadAssetAsync<CoreSettingsContainer>("CoreSettings");
            await handler.Task;
            if (handler.Result == null)
                Debug.LogError("Didn't find core settings");
            _dependencies.Add(typeof(CoreSettings), handler.Result.coreSettings);
        }

        protected override Dictionary<Type, object> GetDependencies()
        {
            return _dependencies;
        }
    }
}