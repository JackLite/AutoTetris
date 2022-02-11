using System;

namespace Global.UI.Timer
{
    public struct TimerComponent
    {
        public readonly int frequency;
        public int currentSeconds;
        public float nextUpdateTime;
        public TimerView view;
        public Action onEnd;

        public TimerComponent(int f)
        {
            frequency = f;
            currentSeconds = 0;
            nextUpdateTime = 0;
            view = null;
            onEnd = null;
        }
    }
}