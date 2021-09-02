using UnityEngine;

namespace SIVS
{
    [RequireComponent(typeof(Camera))]
    public class RainbowAnimationCameraBackground : RainbowAnimationMonoBehaviour
    {
        [Range(0f, 1f)]
        public float saturation;

        [Range(0f, 1f)]
        public float brightness;
        
        private Camera _camera;
        
        private float _currentHue;

        private void Awake() => _camera = GetComponent<Camera>();
        
        private void Update()
        {
            UpdateColor();
        }

        public override void UpdateColor()
        {
            AnimateSingleHue(ref _currentHue);

            _camera.backgroundColor = AnimatedColor(_currentHue, saturation, brightness);
        }
    }
}