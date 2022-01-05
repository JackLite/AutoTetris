using System;

namespace Core.Moving
{
    [Serializable]
    public class MovingData
    {
        public float startFallSpeed = 2f;
        public float manipulationSpeed = 10f;
        public float finishMoveSpeed = 35f;
        public float currentFallSpeed = 2f;
    }
}