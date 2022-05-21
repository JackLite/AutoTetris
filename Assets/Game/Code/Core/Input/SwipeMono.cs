using EcsCore;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.Input
{
    public class SwipeMono : MonoBehaviour, IBeginDragHandler, IDragHandler
    {

        [SerializeField]
        [Tooltip("Процент от ширины экрана")]
        private float _leftThreshold;

        [SerializeField]
        [Tooltip("Процент от ширины экрана")]
        private float _rightThreshold;

        [SerializeField]
        [Tooltip("Процент от высоты экрана")]
        private float _downThreshold;

        private bool _isSwipeStart;
        private Vector2 _swipeStartPoint;

        public void OnBeginDrag(PointerEventData eventData)
        {
            _isSwipeStart = true;
            _swipeStartPoint = eventData.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_isSwipeStart)
                return;

            if (CheckLeftSwipe(eventData.position))
                CreateSwipe(Direction.Left);
            else if (CheckRightSwipe(eventData.position))
                CreateSwipe(Direction.Right);
            else if (CheckDownSwipe(eventData.position))
                CreateSwipe(Direction.Bottom);
        }

        private void CreateSwipe(Direction direction)
        {
            _isSwipeStart = false;
            EcsWorldEventsBlackboard.AddEvent(new InputEvent { direction = direction });
        }

        private bool CheckDownSwipe(in Vector2 pointerEndPosition)
        {
            if (_swipeStartPoint.y < pointerEndPosition.y)
                return false;
            var distanceY = _swipeStartPoint.y - pointerEndPosition.y;
            var screenHeight = Screen.height;
            var distance = distanceY / screenHeight;

            return distance > _downThreshold * .01;
        }

        private bool CheckLeftSwipe(in Vector2 pointerEndPosition)
        {
            if (_swipeStartPoint.x < pointerEndPosition.x)
                return false;

            var distanceX = _swipeStartPoint.x - pointerEndPosition.x;
            var screenWidth = Screen.width;
            var distance = distanceX / screenWidth;

            return distance > _leftThreshold * .01;
        }

        private bool CheckRightSwipe(in Vector2 pointerEndPosition)
        {
            if (_swipeStartPoint.x > pointerEndPosition.x)
                return false;

            var distanceX = pointerEndPosition.x - _swipeStartPoint.x;
            var screenWidth = Screen.width;
            var distance = distanceX / screenWidth;

            return distance > _rightThreshold * .01;
        }

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public void OnLeft(string val)
        {
            float.TryParse(val, out _leftThreshold);
        }

        public void OnDown(string val)
        {
            float.TryParse(val, out _downThreshold);
        }

        public void OnRight(string val)
        {
            float.TryParse(val, out _rightThreshold);
        }
    }
}