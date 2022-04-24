using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Grid.Visual
{
    public class GlowEffectView : MonoBehaviour
    {
        [SerializeField, Range(0.1f, 10f)]
        private float fadeOutDuration;

        [SerializeField]
        private Image image;

        [SerializeField]
        private RectTransform rectTransform;

        private WaitForEndOfFrame _waiter;

        public void SetRow(int row)
        {
            var oldPos = rectTransform.anchoredPosition;
            rectTransform.anchoredPosition = new Vector2(oldPos.x, 75 * row + 75 / 2f);
        }
        
        public void Show(Action<GlowEffectView> callback)
        {
            gameObject.SetActive(true);
            StartCoroutine(ShowCoroutine(callback));
        }

        private IEnumerator ShowCoroutine(Action<GlowEffectView> callback)
        {
            var delta = new Color(0, 0, 0, 1 / fadeOutDuration);
            var startColor = image.color;
            while (image.color.a > 0)
            {
                image.color -= delta * Time.deltaTime;
                yield return _waiter;
            }
            image.color = new Color(startColor.r, startColor.g, startColor.b, 0);
            callback?.Invoke(this);
        }

        public void Reset()
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
            gameObject.SetActive(false);
        }
    }
}