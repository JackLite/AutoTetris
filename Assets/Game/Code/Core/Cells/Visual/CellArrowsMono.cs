using System;
using UnityEngine;

namespace Core.Cells.Visual
{
    /// <summary>
    /// Отвечает за стрелки при подсветке хода
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class CellArrowsMono : MonoBehaviour
    {
        [SerializeField]
        private CellConfig cellConfig;

        [field:SerializeField]
        public Direction Direction { get; private set; }

        private RectTransform[] _arrows;
        private Vector2[] _initPositions;
        // private readonly Vector2 _left = new Vector2(40, 0);
        // private readonly Vector2 _bottom = new Vector2(0, 40);
        // private readonly Vector2 _right = new Vector2(-40, 0);

        private void Awake()
        {
            FillArrows();
            _initPositions = new Vector2[_arrows.Length];
            for (var i = 0; i < _arrows.Length; i++)
            {
                _initPositions[i] = _arrows[i].anchoredPosition;
            }
        }

        private void Update()
        {
            if (Direction == Direction.Right)
                UpdateRight();
            if (Direction == Direction.Bottom)
                UpdateBottom();
            if (Direction == Direction.Left)
                UpdateLeft();
        }

        private void UpdateLeft()
        {
            UpdateArrows(Vector2.left, new Vector2(cellConfig.Distance, 0), v => v.x <= 0);
        }

        private void UpdateBottom()
        {
            UpdateArrows(Vector2.down, new Vector2(0, cellConfig.Distance), v => v.y <= 0);
        }

        private void UpdateRight()
        {
            UpdateArrows(Vector2.right, new Vector2(-cellConfig.Distance, 0), v => v.x >= 0);
        }

        private void UpdateArrows(Vector2 v, Vector2 startPos, Func<Vector2, bool> resetCondition)
        {
            var firstArrow = _arrows[0];
            var lastArrow = _arrows[_arrows.Length - 1];
            foreach (var arrow in _arrows)
            {
                arrow.anchoredPosition += v * cellConfig.MoveSpeed * Time.deltaTime;
            }
            if (resetCondition.Invoke(firstArrow.anchoredPosition))
            {
                lastArrow.SetAsFirstSibling();
                lastArrow.anchoredPosition = firstArrow.anchoredPosition + startPos;
                FillArrows();
            }
        }

        private void FillArrows()
        {
            _arrows ??= new RectTransform[transform.childCount];
            for (var i = 0; i < transform.childCount; i++)
            {
                _arrows[i] = transform.GetChild(i).GetComponent<RectTransform>();
            }
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void ResetArrows()
        {
            if (_arrows == null)
                return;
            for (var i = 0; i < _arrows.Length; i++)
            {
                _arrows[i].anchoredPosition = _initPositions[i];
            }
        }
    }
}