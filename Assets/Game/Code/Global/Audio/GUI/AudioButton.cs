using System;
using UnityEngine;
using UnityEngine.UI;

namespace Global.Audio
{
    public class AudioButton : MonoBehaviour
    {
        [SerializeField]
        private Button _button;

        [SerializeField]
        private GameObject _inactiveIndicator;

        public event Action OnClick;

        private void Awake()
        {
            _button.onClick.AddListener(() => OnClick?.Invoke());
        }

        public void SetState(bool isActive)
        {
            _inactiveIndicator.SetActive(!isActive);
        }
    }
}