using Core;
using EcsCore;
using Global;
using Leopotam.Ecs;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace MainMenu
{
    [EcsSystem(typeof(MainModule))]
    public class StartScreenSystem : IEcsInitSystem
    {
        private StartScreenMono _startScreenMono;
        private EcsWorld _world;
        public void Init()
        {
            _startScreenMono.StartGameButton.OnClick += StartGame;
        }

        private void StartGame()
        {
            Addressables.ReleaseInstance(_startScreenMono.gameObject);
            _world.ActivateModule<CoreModule>();
        }
    }
}