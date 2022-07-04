using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.Figures;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace Core.Cells.Visual
{
    [RequireComponent(typeof(Image))]
    public class CellMono : MonoBehaviour
    {
        [SerializeField]
        private float lightUpOpacity = .5f;

        [SerializeField]
        private Color color;

        [Header("Contour")]
        [SerializeField]
        private Image contour;

        [SerializeField]
        private CellContour[] cellContours;

        [SerializeField]
        private CellArrowsMono[] arrows;

        [SerializeField]
        private Button cellButton;

        [SerializeField]
        private CellVfx cellVfx;

        private RectTransform _rect;
        private Image _image;
        private bool _isOpacityGrow;
        private readonly Dictionary<Direction, CellArrowsMono> _arrowsMap = new Dictionary<Direction, CellArrowsMono>();

        public Sprite CellSprite => _image.sprite;

        public Direction Direction { get; private set; }
        public bool IsLightUp => contour.gameObject.activeSelf;
        
        public event Action DebugCellClick;

        private void Awake()
        {
            _rect = GetComponent<RectTransform>();
            _image = GetComponent<Image>();
            cellButton.onClick.AddListener(() => DebugCellClick?.Invoke());
            foreach (var arrow in arrows)
                _arrowsMap.Add(arrow.Direction, arrow);
        }

        public void SetImageActive(bool isActive)
        {
            _image.enabled = isActive;
        }

        public void SetImage(Sprite sprite)
        {
            _image.sprite = sprite;
        }

        public async void SetImageAsync(AssetReference sprite)
        {
            Sprite s;
            if (sprite.OperationHandle.IsValid() && sprite.OperationHandle.IsDone)
            {
                s = sprite.Asset as Sprite;
            } else if (sprite.OperationHandle.IsValid())
                s = await sprite.OperationHandle.Task as Sprite;
            else
                s = await sprite.LoadAssetAsync<Sprite>().Task;
            _image.sprite = s;
        }

        public void SetPosition(int row, int column)
        {
            _rect.anchoredPosition = new Vector2(column * FigureMono.CELL_SIZE, row * FigureMono.CELL_SIZE);
        }

        public void LightUp(in Figure figure, Direction direction)
        {
            contour.color = cellContours.FirstOrDefault(c => c.direction == direction).color;
            contour.gameObject.SetActive(true);
            ChangeOpacity(lightUpOpacity);
            Direction = direction;
        }

        public void ChangeOpacity(float opacity)
        {
            _image.color = new Color(color.r, color.g, color.b, opacity);
        }

        public void LightDown()
        {
            _image.color = Color.white;
            foreach (var arrow in arrows)
                arrow.Hide();
            contour.gameObject.SetActive(false);
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

        public void PlayVfx()
        {
            cellVfx.PlayFairyDust();
            cellVfx.PlayGlow();
        }

        public void MoveToTopLayer()
        {
            var canvas = gameObject.AddComponent<Canvas>();
            canvas.overrideSorting = true;
            canvas.sortingOrder = 100;
        }

        public void ResetLayer()
        {
            Destroy(gameObject.GetComponent<Canvas>());
        }
    }
}