using System.Collections;
using UnityEngine;

namespace SIVS
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class FreezeFrame : MonoBehaviour
    {
        [Header("Easing")]
        [Range(0f, 2f)]
        public float easeDuration;
        
        public AnimationCurve timeCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        
        private SpriteRenderer _spriteRenderer;

        private Coroutine _freezeFrameCoroutine;

        public delegate void TriggerDelegate();

        public static event TriggerDelegate OnTrigger;
        
        private delegate void ShouldActivateDelegate(float duration);

        private static ShouldActivateDelegate _onShouldActivate;

        public static void Trigger(float duration) => _onShouldActivate?.Invoke(duration);

        private void Awake() => _spriteRenderer = GetComponent<SpriteRenderer>();

        private void OnEnable() => _onShouldActivate += HandleStaticTrigger;

        private void OnDisable() => _onShouldActivate -= HandleStaticTrigger;

        public void Activate(float duration)
        {
            if (_freezeFrameCoroutine != null)
                StopCoroutine(_freezeFrameCoroutine);
            
            OnTrigger?.Invoke();

            _freezeFrameCoroutine = StartCoroutine(FreezeFrameCoroutine(duration));
        }

        private IEnumerator FreezeFrameCoroutine(float duration)
        {
            _spriteRenderer.enabled = true;
            
            yield return EaseTimeScaleCoroutine(true);

            Time.timeScale = 0f;

            yield return new WaitForSecondsRealtime(duration);

            yield return EaseTimeScaleCoroutine();

            Time.timeScale = 1f;
            
            _spriteRenderer.enabled = false;

            _freezeFrameCoroutine = null;
        }

        private IEnumerator EaseTimeScaleCoroutine(bool rightToLeft = false)
        {
            var elapsedTime = 0f;

            while (elapsedTime < easeDuration)
            {
                var time =
                    rightToLeft ? 1f - elapsedTime / easeDuration : elapsedTime / easeDuration;

                Time.timeScale = timeCurve.Evaluate(time);
                
                yield return null;
                
                elapsedTime += Time.unscaledDeltaTime;
            }
        }

        private void HandleStaticTrigger(float duration) => Activate(duration);
    }
}