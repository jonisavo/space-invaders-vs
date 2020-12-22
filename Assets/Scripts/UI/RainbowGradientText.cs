using UnityEngine;
using TMPro;

namespace SIVS
{
    [RequireComponent(typeof(TMP_Text))]
    public class RainbowGradientText : MonoBehaviour
    {
        /// <summary>
        /// Determines the speed of the rainbow gradient animation.
        /// </summary>
        public float animationSpeed = 5.0f;

        public bool animateBottomLeft = false;

        public bool animateBottomRight = false;

        public bool animateTopLeft = false;

        public bool animateTopRight = false;
        
        private TMP_Text _text;

        private float _currentHue = 0;
        
        private void Awake()
        {
            _text = GetComponent<TMP_Text>();
        }

        private void Update()
        {
            _currentHue += animationSpeed * Time.deltaTime;
            if (_currentHue > 360) _currentHue = 0;

            var color = Color.HSVToRGB(_currentHue / 360f, 0.67f, 0.88f);
            
            _text.colorGradient = new VertexGradient(
                animateBottomLeft ? color : _text.colorGradient.bottomLeft,
                animateBottomRight ? color : _text.colorGradient.bottomRight,
                animateTopLeft ? color : _text.colorGradient.topLeft,
                animateTopRight ? color : _text.colorGradient.topRight
            );
        }
    }
}
