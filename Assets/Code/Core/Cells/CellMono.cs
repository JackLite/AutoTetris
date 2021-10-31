using System;
using Core.Figures;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Cells
{
    [RequireComponent(typeof(Image))]
    public class CellMono : MonoBehaviour
    {
        private RectTransform _rect;
        private Image _image;
        private bool _isLightUp;
        
        private void Awake()
        {
            _rect = GetComponent<RectTransform>();
            _image = GetComponent<Image>();
        }

        public void SetImageActive(bool isActive)
        {
            _image.enabled = isActive;
        }

        public void UpdateLight()
        {
            
        }

        public void SetPosition(int row, int column)
        {
            _rect.anchoredPosition = new Vector2(column * FigureMono.CELL_SIZE, row * FigureMono.CELL_SIZE);
        }
    }
}