using UnityEngine;

namespace SIVS
{
    public class RainbowAnimationMonoBehaviour : MonoBehaviour
    {
        [Tooltip("Determines the speed of the rainbow gradient animation.")]
        public float animationSpeed = 5.0f;
        
        public virtual void UpdateColor() {}
        
        public virtual void EnableAllAnimation() {}
        
        public virtual void DisableAllAnimation() {}
        
        protected void AnimateSingleHue(ref float hue)
        {
            hue += animationSpeed * Time.deltaTime;
            if (hue > 360) hue = 0;
        }

        protected Color AnimatedColor(float h360, float s, float v) => Color.HSVToRGB(h360 / 360f, s, v);
    }
}