using System;
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
        private float downSwipeThreshold;

        public void OnPointerDown(PointerEventData eventData)
        {
            _isSwipeStart = true;
            _swipeStartPoint = eventData.position;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!_isSwipeStart)
                return;

            var distanceY = Math.Abs(eventData.position.y - _swipeStartPoint.y);
            var screenHeight = Screen.height;
            var distance = distanceY / screenHeight;

            if (distance < downSwipeThreshold * .01)
                return;
            
            EcsWorldEventsBlackboard.AddEvent(new InputSignal{Type = InputSignalType.Down});
        }
    }
}