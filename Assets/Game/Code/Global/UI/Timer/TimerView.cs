using System.Globalization;
using TMPro;
using UnityEngine;

namespace Global.UI.Timer
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TimerView : MonoBehaviour
    {
        private TextMeshProUGUI _text;
        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
        }

        public void SetTime(int seconds)
        {
            _text.text = seconds.ToString(CultureInfo.InvariantCulture);
        }

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}