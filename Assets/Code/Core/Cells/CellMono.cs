using System;
using Core.Figures;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Cells
{
    [RequireComponent(typeof(Image))]
    public class CellMono : MonoBehaviour
    {
        [SerializeField]
        private float maxOpacity = .7f;

        [SerializeField]
        private float minOpacity = .4f;

        [SerializeField]
        private float speed = .1f;

        [SerializeField]
        private Color color;

        private RectTransform _rect;
        private Image _image;
        private bool _isLightUp;
        private float _currentOpacity;
        private bool _isOpacityGrow;

        private void Awake()
        {
            _rect = GetComponent<RectTransform>();
            _image = GetComponent<Image>();
        }

        private void Start()
        {
            _currentOpacity = minOpacity;
        }

        public void SetImageActive(bool isActive)
        {
            _image.enabled = isActive;
        }

        public void SetPosition(int row, int column)
        {
            _rect.anchoredPosition = new Vector2(column * FigureMono.CELL_SIZE, row * FigureMono.CELL_SIZE);
        }

        public void LightUp()
        {
            _image.enabled = true;
            _isLightUp = true;
        }

        private void Update()
        {
            if (!_isLightUp)
                return;

            UpdateOpacityValue();

            UpdateImage();
        }

        private void UpdateImage()
        {
            _image.color = new Color(color.r, color.g, color.b, _currentOpacity);
        }

        private void UpdateOpacityValue()
        {
            if (_isOpacityGrow)
                _currentOpacity += speed * Time.deltaTime;
            else
                _currentOpacity -= speed * Time.deltaTime;

            if (_currentOpacity < minOpacity && !_isOpacityGrow)
            {
                _isOpacityGrow = true;
                _currentOpacity = minOpacity;
            }
            else if (_currentOpacity > maxOpacity && _isOpacityGrow)
            {
                _isOpacityGrow = false;
                _currentOpacity = maxOpacity;
            }
        }

        public void LightDown()
        {
            _image.color = Color.white;
            _isLightUp = false;
        }
    }
}