using System;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Pause
{
    [RequireComponent(typeof(Button))]
    public class PauseButton : MonoBehaviour
    {
        public event Action OnPause;
        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(() => OnPause?.Invoke());
        }
    }
}