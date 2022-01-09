using Core.Figures;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Cells
{
    [RequireComponent(typeof(Image))]
    public class CellMono : MonoBehaviour
    {
        [SerializeField] 
        private float lightUpOpacity = .5f;

        [SerializeField] 
        private Color color;

        [SerializeField] 
        private CellArrowsMono cellArrows;

        private RectTransform _rect;
        private Image _image;
        private bool _isOpacityGrow;


        public Sprite CellSprite => _image.sprite;

        private void Awake()
        {
            _rect = GetComponent<RectTransform>();
            _image = GetComponent<Image>();
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
            SetImage(figure.Mono.CellSprite);
            SetImageActive(true);
            ChangeOpacity(lightUpOpacity);
            cellArrows.LightUp(direction);
        }

        private void ChangeOpacity(float opacity)
        {
            _image.color = new Color(color.r, color.g, color.b, opacity);
        }

        public void LightDown()
        {
            _image.color = Color.white;
            cellArrows.LightDown();
        }

        public void SetEmpty()
        {
            LightDown();
            SetImageActive(false);
        }
    }
}