using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.GameOver.Views;
using EcsCore;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Core.GameOver
{
    public class GameOverModule : EcsModule
    {
        private GameObject _gameOverScreenObject;
        private GameOverMono _gameOverScreen;
        private readonly Dictionary<Type, object> _dependencies = new Dictionary<Type, object>();
        protected override async Task Setup()
        {
            _gameOverScreenObject = await Addressables.InstantiateAsync("GameOverScreen").Task;
            _gameOverScreen = _gameOverScreenObject.GetComponent<GameOverMono>();
            _dependencies[typeof(GameOverMono)] = _gameOverScreen;
        }
        public override Dictionary<Type, object> GetDependencies()
        {
            return _dependencies;
        }
        public override void Deactivate()
        {
            Addressables.Release(_gameOverScreenObject);
            base.Deactivate();
        }
    }
}