using System;
using Core.Figures;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Cells.Visual
{
    [RequireComponent(typeof(RectTransform))]
    public class ShadowCell : MonoBehaviour
    {
        [SerializeField]
        private Sprite _leftArrow;

        [SerializeField]
        private Sprite _rightArrow;

        [SerializeField]
        private Sprite _bottomArrow;

        [SerializeField]
        private Image _image;

        private Lazy<RectTransform> Rect => new(GetComponent<RectTransform>);
        private Vector2 Delta { get; set; }

        public Vector2 Position => Rect.Value.anchoredPosition;

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Init(in Vector2 startPos, Direction direction)
        {
            SetDirection(direction);
            Rect.Value.anchoredPosition = startPos + Delta;
        }

        public void SetPosition(in Vector2 pos)
        {
            Rect.Value.anchoredPosition = pos;
        }

        private void SetDirection(Direction direction)
        {
            UpdateImage(direction);
            UpdateDelta();
        }
        private void UpdateDelta()
        {
            var sizeDelta = Rect.Value.sizeDelta;
            var xDelta = (FigureMono.CELL_SIZE - sizeDelta.x) / 2;
            var yDelta = (FigureMono.CELL_SIZE - sizeDelta.y) / 2;
            Delta = new Vector2(xDelta, yDelta);
        }
        private void UpdateImage(Direction direction)
        {
            _image.sprite = direction switch
            {
                Direction.Left   => _leftArrow,
                Direction.Bottom => _bottomArrow,
                Direction.Right  => _rightArrow,
                _                => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
            _image.SetNativeSize();
        }
    }
}