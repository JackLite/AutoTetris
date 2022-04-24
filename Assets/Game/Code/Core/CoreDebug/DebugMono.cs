using System;
using UnityEngine;
using UnityEngine.UI;

namespace Core.CoreDebug
{
    public class DebugMono : MonoBehaviour
    {
        [SerializeField]
        private Button startBtn;

        public event Action OnStartClick;

        private void Awake()
        {
            startBtn.onClick.AddListener(() => OnStartClick?.Invoke());
        }
    }
}