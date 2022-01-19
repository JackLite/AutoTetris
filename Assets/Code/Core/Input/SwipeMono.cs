using EcsCore;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.Input
{
    public class SwipeMono : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private bool _isSwipeStart;
        private Vector2 _swipeStartPoint;

        /// <summary>
        /// Порог после которого считается свайп вниз.
        /// Задаётся в процентах от высоты экрана
        /// </summary>
        [SerializeField]
        private float swipeThreshold;

        public void OnPointerDown(PointerEventData eventData)
        {
            _isSwipeStart = true;
            _swipeStartPoint = eventData.position;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!_isSwipeStart)
                return;

            if (CheckDownSwipe(eventData.position))
            {
                EcsWorldEventsBlackboard.AddEvent(new InputEvent { Direction = Direction.Bottom });
                return;
            }
            
            if (CheckLeftSwipe(eventData.position))
            {
                EcsWorldEventsBlackboard.AddEvent(new InputEvent { Direction = Direction.Left });
                return;
            }
            
            if (CheckRightSwipe(eventData.position))
            {
                EcsWorldEventsBlackboard.AddEvent(new InputEvent { Direction = Direction.Right });
                return;
            }
        }

        private bool CheckDownSwipe(in Vector2 pointerEndPosition)
        {
            if (_swipeStartPoint.y < pointerEndPosition.y)
                return false;
            var distanceY = _swipeStartPoint.y - pointerEndPosition.y;
            var screenHeight = Screen.height;
            var distance = distanceY / screenHeight;

            return distance > swipeThreshold * .01;
        }

        private bool CheckLeftSwipe(in Vector2 pointerEndPosition)
        {
            if (_swipeStartPoint.x < pointerEndPosition.x)
                return false;

            var distanceX = _swipeStartPoint.x - pointerEndPosition.x;
            var screenWidth = Screen.width;
            var distance = distanceX / screenWidth;

            return distance > swipeThreshold * .01;
        }

        private bool CheckRightSwipe(in Vector2 pointerEndPosition)
        {
            if (_swipeStartPoint.x > pointerEndPosition.x)
                return false;

            var distanceX = pointerEndPosition.x - _swipeStartPoint.x;
            var screenWidth = Screen.width;
            var distance = distanceX / screenWidth;

            return distance > swipeThreshold * .01;
        }
        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}