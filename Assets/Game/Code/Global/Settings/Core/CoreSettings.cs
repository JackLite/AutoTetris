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

        [Header("Ads")]
        public int adsClearRows = 14;
        public int adsRestoreSpeedTurns = 20;
        [Range(0f, 1f)]
        public float adsSlowFactor = .5f;

        [Header("Debug")]
        public bool aiEnable;

        public CoreFigureTypeToSprite[] figureToSpriteMap;

        public float ManipulationSpeed => manipulationSpeed;
    }
}