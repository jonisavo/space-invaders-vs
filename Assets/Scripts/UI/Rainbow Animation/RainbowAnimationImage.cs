using UnityEngine;
using UnityEngine.UI;

namespace SIVS
{
    [RequireComponent(typeof(Image))]
    public class RainbowAnimationImage : RainbowAnimationMonoBehaviour
    {
        protected bool Active;

        protected float CurrentHue;

        protected Image Image;
        
        private Color _initialColor;

        protected virtual void Awake()
        {
            Image = GetComponent<Image>();
            
            Color.RGBToHSV(Image.color, out CurrentHue, out _, out _);

            _initialColor = Image.color;
        }
        
        protected virtual void Update()
        {
            if (Active)
                UpdateColor();
        }
        
        public override void UpdateColor()
        {
            AnimateSingleHue(ref CurrentHue);

            Image.color = AnimatedColor(CurrentHue, 0.7f, 0.7f);
        }
        
        public override void EnableAllAnimation()
        {
            Active = true;
        }

        public override void DisableAllAnimation()
        {
            Active = false;

            Image.color = _initialColor;
        }

        public void SetHue360(float newHue360) => CurrentHue = newHue360;
    }
}