using System.Collections;
using MF;
using UnityEngine;

namespace SIVS
{
    [RequireComponent(typeof(GradientBackground))]
    public class AnimateGradientBackground : MonoBehaviour
    {
        [Range(0f, 1f)]
        public float speed = 0.1f;

        [Range(0.1f, 3f)]
        public float colorKeyAnimationInterval = 1.5f;

        [Range(0.5f, 20f)]
        public float animationDuration = 8f;

        [Range(0f, 5f)]
        public float waitBetweenAnimations = 1f;

        private GradientBackground _gradientBackground;
        
        private Gradient _gradient;
        
        private GradientColorKey[] _colorKeys;

        private bool[] _animateKeys;

        private void Awake()
        {
            _gradientBackground = GetComponent<GradientBackground>();
            _gradient = _gradientBackground.Gradient;
            _colorKeys = new GradientColorKey[_gradient.colorKeys.Length];
            _animateKeys = new bool[_gradient.colorKeys.Length];
            
            ResetColorKeys();

            UpdateGradientBackground();
        }

        private void Start() => StartCoroutine(AnimateCoroutine());

        private void Update()
        {
            var speedDifference = speed * Time.deltaTime;
            
            for (var i = _colorKeys.Length - 1; i >= 0; i--)
                if (_animateKeys[i])
                    _colorKeys[i].time = Mathf.Clamp(_colorKeys[i].time + speedDifference, 0.0f + i * 0.001f, 1f - i * 0.0025f);

            UpdateGradientBackground();
        }

        private IEnumerator AnimateCoroutine()
        {
            var waitBetweenKeyAnimations = new WaitForSeconds(colorKeyAnimationInterval);

            var waitForAnimationToEnd = new WaitForSeconds(animationDuration);

            var waitBeforeStartingNewAnimation = new WaitForSeconds(waitBetweenAnimations);
            
            while (true)
            {
                for (var i = 0; i < _colorKeys.Length; i++)
                {
                    _animateKeys[i] = true;

                    yield return waitBetweenKeyAnimations;
                }

                yield return waitForAnimationToEnd;
                
                ResetColorKeys();

                yield return waitBeforeStartingNewAnimation;
            }
        }

        private void ResetColorKeys()
        {
            for (var i = 0; i < _gradient.colorKeys.Length; i++)
            {
                _colorKeys[i] = new GradientColorKey(_gradient.colorKeys[i].color, 0f);
                _animateKeys[i] = false;
            }
        }

        private void UpdateGradientBackground()
        {
            _gradientBackground.Gradient.SetKeys(_colorKeys, _gradient.alphaKeys);
            
            _gradientBackground.SetDirty();
        }
    }
}