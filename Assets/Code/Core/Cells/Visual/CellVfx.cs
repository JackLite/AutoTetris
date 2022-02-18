using System;
using UnityEngine;

namespace Core.Cells.Visual
{
    public class CellVfx : MonoBehaviour
    {
        [SerializeField]
        private GameObject vfxFairyDust;

        [SerializeField]
        private float vfxFairyDustDuration;

        private float _deactivateFairyDustTime;

        private void Update()
        {
            if (_deactivateFairyDustTime < Time.time)
                vfxFairyDust.SetActive(false);
        }

        public void PlayFairyDust()
        {
            vfxFairyDust.SetActive(true);
            _deactivateFairyDustTime = Time.time + vfxFairyDustDuration;
        }

        public void PlayGlow()
        {
        }
    }
}