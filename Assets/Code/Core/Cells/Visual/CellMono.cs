﻿using System;
using Core.Figures;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Cells.Visual
{
    [RequireComponent(typeof(Image))]
    public class CellMono : MonoBehaviour
    {
        [SerializeField] private float lightUpOpacity = .5f;

        [SerializeField] private Color color;

        [SerializeField] private CellArrowsMono cellArrows;

        [SerializeField] private Button cellButton;

        [SerializeField] private CellBorder[] borders;

        private RectTransform _rect;
        private Image _image;
        private bool _isOpacityGrow;

        public Sprite CellSprite => _image.sprite;

        public event Action DebugCellClick;

        private void Awake()
        {
            _rect = GetComponent<RectTransform>();
            _image = GetComponent<Image>();
            cellButton.onClick.AddListener(() => DebugCellClick?.Invoke());
        }

        public void SetImageActive(bool isActive)
        {
            _image.enabled = isActive;
        }

        public void SetImage(Sprite sprite)
        {
            _image.sprite = sprite;
        }

        public void SetPosition(int row, int column)
        {
            _rect.anchoredPosition = new Vector2(column * FigureMono.CELL_SIZE, row * FigureMono.CELL_SIZE);
        }

        public void LightUp(in Figure figure, Direction direction)
        {
            SetImage(figure.mono.CellSprite);
            SetImageActive(true);
            ChangeOpacity(lightUpOpacity);
            cellArrows.LightUp(direction);
        }

        public void ChangeOpacity(float opacity)
        {
            _image.color = new Color(color.r, color.g, color.b, opacity);
        }

        public void LightDown()
        {
            _image.color = Color.white;
            cellArrows.LightDown();
            ShowBorders(Direction.None);
        }

        public void SetEmpty()
        {
            LightDown();
            SetImageActive(false);
        }

        public void SetButtonState(bool isBtnActive)
        {
            cellButton.interactable = isBtnActive;
            SetImageActive(true);
            ChangeOpacity(.5f);
        }

        public void ShowBorders(Direction directionMask)
        {
            foreach (var border in borders)
                border.borderObject.SetActive((directionMask & border.direction) > 0);
        }
    }
}