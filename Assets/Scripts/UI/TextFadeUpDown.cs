using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace SIVS
{
    [RequireComponent(typeof(TMP_Text))]
    public class TextFadeUpDown : MonoBehaviour
    {
        [Tooltip("The speed of the fading animation.")]
        [Min(0.1f)]
        public float speed = 4f;

        [Tooltip("Seconds to wait between fades.")]
        [Min(0f)]
        public float waitBetweenFades = 1f;
        
        private TMP_Text _text;

        private void Awake() => _text = GetComponent<TMP_Text>();
        
        private Coroutine _fadeCoroutine;

        private void OnEnable()
        {
            _fadeCoroutine = StartCoroutine(FadeCoroutine());
        }

        private void OnDisable()
        {
            StopCoroutine(_fadeCoroutine);
            _fadeCoroutine = null;
            _text.alpha = 0;
        }

        private IEnumerator FadeCoroutine()
        {
            var waitBetween = new WaitForSeconds(waitBetweenFades);
            
            while (true)
            {
                yield return FadeTo(1f);

                yield return waitBetween;

                yield return FadeTo(0f);

                yield return waitBetween;
            }
        }

        private IEnumerator FadeTo(float target)
        {
            var distance = target - _text.alpha;

            while (Math.Abs(_text.alpha - target) > 0.01f)
            {
                _text.alpha += speed * Time.deltaTime * Mathf.Sign(distance);
                
                yield return null;
            }
        }
    }
}