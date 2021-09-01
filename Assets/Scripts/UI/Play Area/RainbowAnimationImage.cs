using UnityEngine;
using UnityEngine.UI;

namespace SIVS
{
    [RequireComponent(typeof(Image))]
    public class RainbowAnimationImage : RainbowAnimationMonoBehaviour
    {
        protected Color _initialColor;
        
        protected bool _active;

        protected float _currentHue;

        protected Image _image;

        protected virtual void Awake()
        {
            _image = GetComponent<Image>();
            
            Color.RGBToHSV(_image.color, out _currentHue, out _, out _);

            _initialColor = _image.color;
        }
        
        protected virtual void Update()
        {
            if (_active)
                UpdateColor();
        }
        
        public override void UpdateColor()
        {
            AnimateSingleHue(ref _currentHue);

            _image.color = Color.HSVToRGB(_currentHue / 360f, 0.7f, 0.7f);
        }
        
        public override void EnableAllAnimation()
        {
            _active = true;
        }

        public override void DisableAllAnimation()
        {
            _active = false;

            _image.color = _initialColor;
        }
    }
}