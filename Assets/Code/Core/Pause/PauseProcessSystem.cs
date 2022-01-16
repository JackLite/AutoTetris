using Core.Pause.Signals;
using EcsCore;
using Leopotam.Ecs;
using UnityEngine;

namespace Core.Pause
{
    [EcsSystem(typeof(CoreModule))]
    public class PauseProcessSystem : IEcsRunSystem, IEcsDestroySystem
    {
        private CoreState _coreState;
        private EcsEventTable _eventTable;

        public void Run()
        {
            if (_eventTable.Has<PauseSignal>())
                Pause();
            if (_eventTable.Has<UnpauseSignal>())
                Unpause();
        }

        private void Pause()
        {
            _coreState.IsPaused = true;
            Time.timeScale = 0;
        }

        private void Unpause()
        {
            _coreState.IsPaused = false;
            Time.timeScale = 1;
        }
        public void Destroy()
        {
            Unpause();
        }
    }
}