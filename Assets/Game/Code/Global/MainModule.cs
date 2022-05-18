using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.AI.Genetic;
using EcsCore;
using Global.Ads;
using Global.Audio;
using Global.Leaderboard.Services;
using Global.Saving;
using Global.Settings;
using Global.Settings.Core;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using MainMenu;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Audio;

namespace Global
{
    [EcsGlobalModule]
    public class MainModule : EcsModule
    {
        private readonly Dictionary<Type, object> _dependencies;

        public MainModule()
        {
            _dependencies = new Dictionary<Type, object>();
        }

        protected override async Task Setup()
        {
            if (!Debug.isDebugBuild)
                Debug.unityLogger.filterLogType = LogType.Error;

            PlayGamesPlatform.Instance.Authenticate(ProcessAuth);
            
            _dependencies.Add(typeof(PlayerData), new PlayerData());
            _dependencies.Add(typeof(AdsService), new AdsService());
            _dependencies.Add(typeof(StartCoreData), new StartCoreData());
            _dependencies.Add(typeof(SaveService), new SaveService());
            _dependencies.Add(typeof(AiGeneticService), new AiGeneticService());
            _dependencies[typeof(ScoresService)] = new ScoresService();
            _dependencies[typeof(SelectScoresService)] = new SelectScoresService();
            await LoadCoreSettings();
            var globalSettings = await LoadGlobalSettings();
            _dependencies[typeof(FakeScoresService)] = new FakeScoresService(globalSettings.fakeScores.text);
            _dependencies[typeof(AudioService)] = new AudioService(globalSettings.mixer);
            EcsWorldContainer.World.ActivateModule<MainMenuModule>();
        }

        private void ProcessAuth(SignInStatus status)
        {
            Debug.Log("ProcessAuth: " + status);
            if (status == SignInStatus.Success)
            {
                Debug.Log("Success auth. User: " + PlayGamesPlatform.Instance.localUser.userName);
            }
        }

        private async Task LoadCoreSettings()
        {
            var result = await LoadSettings<CoreSettingsContainer>("CoreSettings");
            _dependencies.Add(typeof(CoreSettings), result.coreSettings);
        }

        private async Task<GlobalSettings> LoadGlobalSettings()
        {
            var result = await LoadSettings<GlobalSettingsContainer>("GlobalSettings");
            _dependencies.Add(typeof(GlobalSettings), result.globalSettings);
            return result.globalSettings;
        }

        private static async Task<T> LoadSettings<T>(string address)
        {
            var handler = Addressables.LoadAssetAsync<T>(address);
            await handler.Task;
            if (handler.Result == null)
                Debug.LogError("Didn't find core settings");
            return handler.Result;
        }

        public override Dictionary<Type, object> GetDependencies()
        {
            return _dependencies;
        }
    }
}