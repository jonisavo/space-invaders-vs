using UnityEngine;
using TMPro;

namespace SIVS
{
    [RequireComponent(typeof(TMP_Text))]
    public class RainbowGradientText : MonoBehaviour
    {
        [Tooltip("Determines the speed of the rainbow gradient animation.")]
        public float animationSpeed = 5.0f;
        
        [Tooltip("Toggles whether the bottom left portion is animated.")]
        public bool animateBottomLeft = false;
        
        [Tooltip("The initial hue for the bottom left portion.")]
        public float initialBottomLeftHue = 0;
        
        [Tooltip("Toggles whether the bottom right portion is animated.")]
        public bool animateBottomRight = false;
        
        [Tooltip("The initial hue for the bottom right portion.")]
        public float initialBottomRightHue = 0;
        
        [Tooltip("Toggles whether the top left portion is animated.")]
        public bool animateTopLeft = false;
        
        [Tooltip("The initial hue for the top left portion.")]
        public float initialTopLeftHue = 0;
        
        [Tooltip("Toggles whether the top right portion is animated.")]
        public bool animateTopRight = false;
        
        [Tooltip("The initial hue for the top right portion.")]
        public float initialTopRightHue = 0;
        
        private TMP_Text _text;
        
        private float _currentTopRightHue = 0;

        private float _currentBottomRightHue = 0;

        private float _currentTopLeftHue = 0;

        private float _currentBottomLeftHue = 0;
        
        private void Awake()
        {
            _text = GetComponent<TMP_Text>();
            _currentTopRightHue = initialTopLeftHue;
            _currentBottomRightHue = initialBottomRightHue;
            _currentTopLeftHue = initialTopLeftHue;
            _currentBottomLeftHue = initialBottomLeftHue;
        }

        private void Update()
        {
            AnimateHues();

            _text.colorGradient = new VertexGradient(
                animateTopLeft ?
                    Color.HSVToRGB(_currentTopRightHue / 360f, 0.67f, 0.88f) : _text.colorGradient.topLeft,
                animateTopRight ? 
                    Color.HSVToRGB(_currentTopRightHue / 360f, 0.67f, 0.88f) : _text.colorGradient.topRight,
                animateBottomLeft ?
                    Color.HSVToRGB(_currentBottomLeftHue / 360f, 0.67f, 0.7f) : _text.colorGradient.bottomLeft,
                animateBottomRight ? 
                    Color.HSVToRGB(_currentBottomRightHue / 360f, 0.67f, 0.7f) : _text.colorGradient.bottomRight
            );
        }

        private void AnimateHues()
        {
            if (animateBottomLeft)
                AnimateSingleHue(ref _currentBottomLeftHue);
            
            if (animateBottomRight)
                AnimateSingleHue(ref _currentBottomRightHue);
            
            if (animateTopLeft)
                AnimateSingleHue(ref _currentTopLeftHue);
            
            if (animateTopRight)
                AnimateSingleHue(ref _currentTopRightHue);
        }

        private void AnimateSingleHue(ref float hue)
        {
            hue += animationSpeed * Time.deltaTime;
            if (hue > 360) hue = 0;
        }
    }
}
