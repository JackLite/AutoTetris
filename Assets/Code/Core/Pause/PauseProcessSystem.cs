using EcsCore;
using Leopotam.Ecs;
using UnityEngine;

namespace Core.Pause
{
    [EcsSystem(typeof(CoreModule))]
    public class PauseProcessSystem : IEcsRunSystem
    {
        private CoreState _coreState;

        private MainScreenMono _mainScreenMono;

        private EcsEventTable _eventTable;

        public void Run()
        {
            if (_eventTable.IsEventExist<PauseSignal>())
                Pause();
            if (_eventTable.IsEventExist<UnpauseSignal>())
                Unpause();
        }

        private void Pause()
        {
            _coreState.IsPaused = true;
            Time.timeScale = 0;
            _mainScreenMono.PauseScreen.SetActive(true);
        }

        private void Unpause()
        {
            _coreState.IsPaused = false;
            Time.timeScale = 1;
            _mainScreenMono.PauseScreen.SetActive(false);
        }
    }
}