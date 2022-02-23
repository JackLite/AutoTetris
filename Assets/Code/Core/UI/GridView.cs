using System.Threading.Tasks;
using UnityEngine;

namespace Core.UI
{
    public class GridView : MonoBehaviour
    {
        [SerializeField]
        private RectTransform rectTransform;

        [SerializeField]
        private Vector2 delta = Vector2.down;

        [SerializeField]
        private int durationMs = 100;
        
        public async void PlayFallEffect()
        {
            var oldPos = rectTransform.anchoredPosition;
            rectTransform.anchoredPosition = oldPos + delta;
            await Task.Delay(durationMs);
            rectTransform.anchoredPosition = oldPos;
        }
    }
}