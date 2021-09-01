using System;
using UnityEngine;
using TMPro;

namespace SIVS
{
    public class RainbowAnimationTextGradient : RainbowAnimationText
    {
        [Serializable]
        public struct PortionColorSettings
        {
            [Tooltip("Toggles whether the portion is animated.")]
            public bool animate;
            [Tooltip("Use the current hue as the initial value.")]
            public bool useCurrentHueAsInitial;
            [Range(0.0f, 1.0f)]
            [Tooltip("The initial hue for the portion.")]
            public float initialHue;
            [Range(0.0f, 1.0f)]
            [Tooltip("The saturation of the portion.")]
            public float saturation;
            [Tooltip("The brightness of the portion.")]
            [Range(0.0f, 1.0f)]
            public float brightness;
        }

        [Header("Color Gradient Settings")]
        public PortionColorSettings bottomLeft = new PortionColorSettings
        {
            animate = false, initialHue = 0f, saturation = 0.7f, brightness = 0.67f
        };

        public PortionColorSettings bottomRight = new PortionColorSettings
        {
            animate = false, initialHue = 0f, saturation = 0.7f, brightness = 0.67f
        };

        public PortionColorSettings topLeft = new PortionColorSettings
        {
            animate = false, initialHue = 0f, saturation = 0.88f, brightness = 0.67f
        };

        public PortionColorSettings topRight = new PortionColorSettings
        {
            animate = false, initialHue = 0f, saturation = 0.88f, brightness = 0.67f
        };

        private float _currentTopRightHue;

        private float _currentBottomRightHue;

        private float _currentTopLeftHue;

        private float _currentBottomLeftHue;

        private VertexGradient _initialGradient;

        protected override void LoadInitialHues()
        {
            _initialGradient = _text.colorGradient;
            
            _currentTopRightHue = GetInitialHue(topRight, _initialGradient.topRight);
            _currentBottomRightHue = GetInitialHue(bottomRight, _initialGradient.bottomRight);
            _currentTopLeftHue = GetInitialHue(topLeft, _initialGradient.topLeft);
            _currentBottomLeftHue = GetInitialHue(bottomLeft, _initialGradient.bottomLeft);
        }

        private float GetInitialHue(PortionColorSettings portion, Color gradientColor)
        {
            if (!portion.useCurrentHueAsInitial)
                return portion.initialHue;

            Color.RGBToHSV(gradientColor, out var hue, out _, out _);

            return hue;
        }

        public override void UpdateColor()
        {
            AnimateHues();

            _text.colorGradient = new VertexGradient(
                topLeft.animate ?
                    AnimatedColor(_currentTopRightHue, topLeft.saturation, topLeft.brightness) : _text.colorGradient.topLeft,
                topRight.animate ? 
                    AnimatedColor(_currentTopRightHue, topRight.saturation, topRight.brightness) : _text.colorGradient.topRight,
                bottomLeft.animate ?
                    AnimatedColor(_currentBottomLeftHue, bottomLeft.saturation, bottomLeft.brightness) : _text.colorGradient.bottomLeft,
                bottomRight.animate ? 
                    AnimatedColor(_currentBottomRightHue, bottomRight.saturation, bottomRight.brightness) : _text.colorGradient.bottomRight
            );
        }

        public override void EnableAllAnimation()
        {
            active = true;
            topLeft.animate = true;
            topRight.animate = true;
            bottomLeft.animate = true;
            bottomRight.animate = true;
        }

        public override void DisableAllAnimation()
        {
            active = false;
            topLeft.animate = false;
            topRight.animate = false;
            bottomLeft.animate = false;
            bottomRight.animate = false;

            _text.colorGradient = _initialGradient;
        }

        private void AnimateHues()
        {
            if (bottomLeft.animate)
                AnimateSingleHue(ref _currentBottomLeftHue);
            
            if (bottomRight.animate)
                AnimateSingleHue(ref _currentBottomRightHue);
            
            if (topLeft.animate)
                AnimateSingleHue(ref _currentTopLeftHue);
            
            if (topRight.animate)
                AnimateSingleHue(ref _currentTopRightHue);
        }
    }
}
