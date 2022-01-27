using System;

namespace Core.Moving
{
    [Serializable]
    public class MovingData
    {
        public float startFallSpeed = 200f;
        public float manipulationSpeed = 1000f;
        public float finishMoveSpeed = 3500f;
        public float currentFallSpeed = 200f;
    }
}