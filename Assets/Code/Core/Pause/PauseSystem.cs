using EcsCore;
using Leopotam.Ecs;
using UnityEngine;

namespace Core.Pause
{
    [EcsSystem(typeof(CoreModule))]
    public class PauseSystem : IEcsInitSystem, IEcsDestroySystem
    {
        private MainScreenMono _mainScreenMono;
        private EcsWorld _world;
        private CoreState _coreState;
        
        public void Init()
        {
            _mainScreenMono.PauseButton.onClick.AddListener(OnPause);
            _mainScreenMono.UnPauseButton.onClick.AddListener(OnUnPause);
        }

        private void OnPause()
        {
            _coreState.IsPaused = true;
            Time.timeScale = 0;
            _mainScreenMono.PauseScreen.SetActive(true);
        }

        private void OnUnPause()
        {
            _coreState.IsPaused = false;
            Time.timeScale = 1;
            _mainScreenMono.PauseScreen.SetActive(false);
        }

        public void Destroy()
        {
            _mainScreenMono.PauseButton.onClick.RemoveListener(OnPause);
            _mainScreenMono.UnPauseButton.onClick.AddListener(OnUnPause);

            Time.timeScale = 1;
        }
    }
}