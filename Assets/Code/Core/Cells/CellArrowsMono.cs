using UnityEngine;

namespace Core.Cells
{
    /// <summary>
    /// Отвечает за стрелки при подсветке хода
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class CellArrowsMono : MonoBehaviour
    {
        [SerializeField]
        private RectTransform firstArrowTransform;

        [SerializeField]
        private RectTransform secondArrowTransform;

        [SerializeField]
        private CellConfig cellConfig;

        private Direction _currentDirection = Direction.Down;
        private float _thresholdSqr;
        private Vector2 _cellSize;

        private void Start()
        {
            _cellSize = GetComponent<RectTransform>().sizeDelta;
            _thresholdSqr = _cellSize.x * _cellSize.x;
            firstArrowTransform.gameObject.SetActive(false);
        }

        private void Update()
        {
            var delta = cellConfig.MoveSpeed * Time.deltaTime;
            MoveArrow(firstArrowTransform, GetVDirection(_currentDirection), delta);
            MoveArrow(secondArrowTransform, GetVDirection(_currentDirection), delta);
        }

        private void MoveArrow(RectTransform arrow, Vector2 direction, float delta)
        {
            arrow.anchoredPosition += direction * delta;
            if (arrow.anchoredPosition.sqrMagnitude > _thresholdSqr)
            {
                arrow.anchoredPosition = -arrow.anchoredPosition + direction * delta;
            }
        }

        public void LightUp(Direction direction)
        {
            _currentDirection = direction;

            SetArrowRotation(firstArrowTransform, direction);
            SetArrowRotation(secondArrowTransform, direction);

            ResetArrowToCenter(firstArrowTransform);
            ResetArrowToCenter(secondArrowTransform);

            var dirVector = GetVDirection(direction);
            var size = firstArrowTransform.sizeDelta.x;
            firstArrowTransform.anchoredPosition = dirVector * size;
            secondArrowTransform.anchoredPosition = dirVector * (size - _cellSize.x);

            firstArrowTransform.gameObject.SetActive(true);
            secondArrowTransform.gameObject.SetActive(true);
        }

        public void LightDown()
        {
            SetArrowsActive(false);
        }

        private static void ResetArrowToCenter(RectTransform arrow)
        {
            arrow.anchoredPosition = Vector2.zero;
        }

        private static void SetArrowRotation(Transform arrow, Direction direction)
        {
            var angle = GetAngle(direction);
            arrow.localRotation = Quaternion.Euler(0, 0, angle);
        }

        private static float GetAngle(Direction direction)
        {
            return direction switch
            {
                Direction.Left  => 180,
                Direction.Down  => -90,
                Direction.Right => 0
            };
        }


        private static Vector2 GetVDirection(Direction direction)
        {
            return direction switch
            {
                Direction.Left  => Vector2.left,
                Direction.Down  => Vector2.down,
                Direction.Right => Vector2.right
            };
        }

        private void SetArrowsActive(bool isActive)
        {
            firstArrowTransform.gameObject.SetActive(isActive);
            secondArrowTransform.gameObject.SetActive(isActive);
        }

    }
}