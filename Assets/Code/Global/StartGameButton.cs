using System;
using UnityEngine;
using UnityEngine.UI;

namespace Global
{
    [RequireComponent(typeof(Button))]
    public class StartGameButton : MonoBehaviour
    {
        private Button _button;

        public event Action OnClick;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void Start()
        {
            _button.onClick.AddListener(() => OnClick?.Invoke());
        }
    }
}