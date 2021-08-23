using UnityEngine;
using TMPro;

namespace SIVS
{
    [RequireComponent(typeof(TMP_Text))]
    public class RainbowGradientText : RainbowGradientMonoBehaviour
    {
        [Tooltip("Toggles whether the bottom left portion is animated.")]
        public bool animateBottomLeft;
        
        [Tooltip("The initial hue for the bottom left portion.")]
        public float initialBottomLeftHue;
        
        [Tooltip("Toggles whether the bottom right portion is animated.")]
        public bool animateBottomRight;
        
        [Tooltip("The initial hue for the bottom right portion.")]
        public float initialBottomRightHue;
        
        [Tooltip("Toggles whether the top left portion is animated.")]
        public bool animateTopLeft;
        
        [Tooltip("The initial hue for the top left portion.")]
        public float initialTopLeftHue;
        
        [Tooltip("Toggles whether the top right portion is animated.")]
        public bool animateTopRight;
        
        [Tooltip("The initial hue for the top right portion.")]
        public float initialTopRightHue;
        
        private TMP_Text _text;
        
        private float _currentTopRightHue;

        private float _currentBottomRightHue;

        private float _currentTopLeftHue;

        private float _currentBottomLeftHue;
        
        private void Awake()
        {
            _text = GetComponent<TMP_Text>();
            _currentTopRightHue = initialTopRightHue;
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
    }
}
