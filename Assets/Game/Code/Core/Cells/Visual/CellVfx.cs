using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Cells.Visual
{
    public class CellVfx : MonoBehaviour
    {
        [Header("Fairy Dust")]
        [SerializeField]
        private GameObject vfxFairyDust;

        [SerializeField]
        private float vfxFairyDustDuration;

        [Header("Glow")]
        [SerializeField]
        private Image glowImage;

        [SerializeField]
        private RectTransform glowRect;

        [SerializeField]
        private float glowDuration;

        [SerializeField]
        private float glowScaleStep;
        
        private readonly WaitForEndOfFrame _waiter = new WaitForEndOfFrame();

        public async void PlayFairyDust()
        {
            vfxFairyDust.SetActive(true);
            await Task.Delay((int) (vfxFairyDustDuration * 1000));
            if (vfxFairyDust)
                vfxFairyDust.SetActive(false);
        }

        public void PlayGlow()
        {
            StartCoroutine(PlayGlowCoroutine());
        }

        private IEnumerator PlayGlowCoroutine()
        {
            glowImage.gameObject.SetActive(true);
            glowRect.localScale = Vector3.one;
            var count = 0f;
            while (count <= glowDuration)
            {
                glowRect.localScale += Vector3.one * glowScaleStep * Time.deltaTime;
                count += Time.deltaTime;
                yield return _waiter;
            }
            glowImage.gameObject.SetActive(false);
        }
    }
}