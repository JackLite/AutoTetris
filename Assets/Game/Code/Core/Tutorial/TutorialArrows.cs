using System;
using UnityEngine;

namespace Core.Tutorial
{
    public class TutorialArrows : MonoBehaviour
    {
        [SerializeField]
        private GameObject _rightArrow;

        [SerializeField]
        private GameObject _leftArrow;

        [SerializeField]
        private GameObject _bottomArrow;

        public void ShowArrow(Direction direction)
        {
            _rightArrow.SetActive(false);
            _leftArrow.SetActive(false);
            _bottomArrow.SetActive(false);

            var arr = direction switch
            {
                Direction.Right => _rightArrow,
                Direction.Left => _leftArrow,
                Direction.Bottom => _bottomArrow,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), "Wrong direction for tutorial arrow")
            };

            arr.SetActive(true);
        }
    }
}