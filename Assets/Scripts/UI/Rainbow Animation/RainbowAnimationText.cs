﻿using UnityEngine;
using TMPro;

namespace SIVS
{
    [RequireComponent(typeof(TMP_Text))]
    public class RainbowAnimationText : RainbowAnimationMonoBehaviour
    {
        [Tooltip("Whether the animation is active.")]
        public bool active;

        [Range(0.0f, 1.0f)]
        [Tooltip("The saturation value of the color.")]
        public float colorSaturation = 0.88f;

        [Range(0.0f, 1.0f)]
        [Tooltip("The brightness value (V in HSV) of the color.")]
        public float colorBrightness = 0.67f;

        protected TMP_Text Text;

        private float _currentSingleColorHue;

        private Color _initialSingleColor;

        protected void Awake()
        {
            Text = GetComponent<TMP_Text>();
            
            LoadInitialHues();
        }
        
        protected virtual void LoadInitialHues()
        {
            Color.RGBToHSV(Text.color, out _currentSingleColorHue, out _, out _);
            _initialSingleColor = Text.color;
        }

        protected void Update()
        {
            if (active)
                UpdateColor();
        }

        public override void UpdateColor()
        {
            AnimateSingleHue(ref _currentSingleColorHue);

            Text.color = Color.HSVToRGB(_currentSingleColorHue / 360f, colorSaturation, colorBrightness);
        }

        public override void EnableAllAnimation()
        {
            active = true;
        }

        public override void DisableAllAnimation()
        {
            active = false;

            Text.color = _initialSingleColor;
        }
    }
}
