﻿using Global.Settings.Core;

namespace Core.Moving
{
    public class MovingData
    {
        public float currentFallSpeed = 2f;
        public float factor = 1f;
        public bool isMoveAllowed = true;
        public CoreSpeedProgression currentDifficult;
    }
}