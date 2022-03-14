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
using UnityEngine.ResourceManagement.AsyncOperations;

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
            await LoadCoreSettings();
            await LoadGlobalSettings();
        }

        private async Task LoadCoreSettings()
        {
            var result = await LoadSettings<CoreSettingsContainer>("CoreSettings");
            _dependencies.Add(typeof(CoreSettings), result.coreSettings);
        }

        private async Task LoadGlobalSettings()
        {
            var result = await LoadSettings<GlobalSettingsContainer>("GlobalSettings");
            _dependencies.Add(typeof(GlobalSettings), result.globalSettings);
        }

        private static async Task<T> LoadSettings<T>(string address)
        {
            var handler = Addressables.LoadAssetAsync<T>(address);
            await handler.Task;
            if (handler.Result == null)
                Debug.LogError("Didn't find core settings");
            return handler.Result;
        }

        protected override Dictionary<Type, object> GetDependencies()
        {
            return _dependencies;
        }
    }
}