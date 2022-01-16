using Core.Pause.Signals;
using EcsCore;
using Leopotam.Ecs;

namespace Core.Pause
{
    [EcsSystem(typeof(CoreModule))]
    public class PauseScreenSystem : IEcsRunSystem
    {
        private MainScreenMono _mainScreenMono;
        private EcsEventTable _eventTable;

        public void Run()
        {
            if (_eventTable.Has<ShowPauseScreenSignal>())
                _mainScreenMono.PauseScreen.SetActive(true);

            if (_eventTable.Has<HidePauseScreenSignal>())
                _mainScreenMono.PauseScreen.SetActive(false);
        }
    }
}