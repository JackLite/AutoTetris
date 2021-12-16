using System;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    [RequireComponent(typeof(Button))]
    public class StartGameButton : MonoBehaviour
    {
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void Start()
        {
            _button.onClick.AddListener(() => OnClick?.Invoke());
        }

        public event Action OnClick;
    }
}