using System;
using UnityEngine;

namespace Global.Settings.Core
{
    [Serializable]
    public class CoreSettings
    {
        [SerializeField]
        private float manipulationSpeed = 10f;

        public float finishMoveMinSpeed = 35f;
        public float finishMoveMaxSpeed = 35f;
        public AnimationCurve finishMoveVelocity;
        public CoreSpeedProgression[] fallSpeedProgression;

        public int adsClearRows = 14;

        public float ManipulationSpeed => manipulationSpeed;
        
    }
}