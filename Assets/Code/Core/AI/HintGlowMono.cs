using UnityEngine;
using UnityEngine.UI;

namespace Core.AI
{
    public class HintGlowMono : MonoBehaviour
    {
        [SerializeField]
        private float maxOpacity = .7f;

        [SerializeField]
        private float minOpacity = .4f;

        [SerializeField]
        private float speed = .1f;

        [SerializeField]
        private Color color;
        
        [SerializeField]
        private Image[] images;
        

        private float _currentOpacity;
        private bool _isOpacityGrow;

        private void Start()
        {
            _currentOpacity = minOpacity;
        }

        private void Update()
        {
            UpdateOpacityValue();

            UpdateImages();
        }

        private void UpdateImages()
        {
            foreach (var image in images)
            {
                image.color = new Color(color.r, color.g, color.b, _currentOpacity);
            }
        }

        private void UpdateOpacityValue()
        {
            if (_isOpacityGrow)
                _currentOpacity += speed * Time.deltaTime;
            else
                _currentOpacity -= speed * Time.deltaTime;

            if (_currentOpacity < minOpacity && !_isOpacityGrow)
            {
                _isOpacityGrow = true;
                _currentOpacity = minOpacity;
            }
            else if (_currentOpacity > maxOpacity && _isOpacityGrow)
            {
                _isOpacityGrow = false;
                _currentOpacity = maxOpacity;
            }
        }
    }
}