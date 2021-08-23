using UnityEngine;

namespace SIVS
{
    public class RainbowGradientMonoBehaviour : MonoBehaviour
    {
        [Tooltip("Determines the speed of the rainbow gradient animation.")]
        public float animationSpeed = 5.0f;
        
        protected void AnimateSingleHue(ref float hue)
        {
            hue += animationSpeed * Time.deltaTime;
            if (hue > 360) hue = 0;
        }
    }
}