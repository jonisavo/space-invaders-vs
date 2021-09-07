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
        
        [Header("Time")]
        public AnimationCurve timeCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        
        [Header("Sprite")]
        public AnimationCurve spriteScaleCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        [Range(1f, 20f)]
        public float spriteMaxScale = 10.0f;

        [Min(0f)]
        public float spriteRotateSpeed = 2f;

        [Header("Shockwave")]
        public AnimationCurve shockwaveScaleCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        public Gradient shockwaveColorGradient = new Gradient();

        [Range(0.5f, 5f)]
        public float shockwaveDuration = 1.5f;

        [Range(1f, 25f)]
        public float shockwaveMaxScale = 12.0f;
        
        private SpriteRenderer _spriteRenderer;

        private Coroutine _freezeFrameCoroutine;

        public delegate void TriggerDelegate();

        public static event TriggerDelegate OnTrigger;
        
        private delegate void ShouldActivateDelegate(Vector2 position, float duration);

        private static ShouldActivateDelegate _onShouldActivate;

        public static void Trigger(Vector2 position, float duration) => _onShouldActivate?.Invoke(position, duration);

        private void Awake() => _spriteRenderer = GetComponent<SpriteRenderer>();

        private void OnEnable() => _onShouldActivate += HandleStaticTrigger;

        private void OnDisable() => _onShouldActivate -= HandleStaticTrigger;

        public void Activate(Vector2 position, float duration)
        {
            if (_freezeFrameCoroutine != null)
                StopCoroutine(_freezeFrameCoroutine);
            
            OnTrigger?.Invoke();

            _freezeFrameCoroutine = StartCoroutine(FreezeFrameCoroutine(position, duration));
        }

        private IEnumerator FreezeFrameCoroutine(Vector2 position, float duration)
        {
            var rotateCoroutine = StartCoroutine(RotateSpriteCoroutine());
            
            transform.localScale = Vector3.zero;

            transform.position = new Vector3(position.x, position.y, -3f);
            
            _spriteRenderer.enabled = true;

            StartCoroutine(CreateShockwaveCoroutine());
            
            yield return EaseTimeScaleCoroutine(true);

            Time.timeScale = timeCurve.keys[0].value;

            yield return new WaitForSecondsRealtime(duration);

            yield return EaseTimeScaleCoroutine();
            
            StopCoroutine(rotateCoroutine);

            Time.timeScale = timeCurve.keys[timeCurve.keys.Length - 1].value;
            
            _spriteRenderer.enabled = false;

            _freezeFrameCoroutine = null;
        }

        private IEnumerator CreateShockwaveCoroutine()
        {
            var shockwaveObj = new GameObject("Shockwave", typeof(SpriteRenderer));
            shockwaveObj.transform.SetParent(transform);
            shockwaveObj.transform.localScale = Vector3.zero;

            var shockwaveSpriteRenderer = shockwaveObj.GetComponent<SpriteRenderer>();

            shockwaveSpriteRenderer.sprite = _spriteRenderer.sprite;
            
            var elapsedTime = 0f;

            var maxShockwaveScale = Vector3.one * shockwaveMaxScale;

            while (elapsedTime < shockwaveDuration)
            {
                var time = elapsedTime / shockwaveDuration;

                shockwaveSpriteRenderer.color = shockwaveColorGradient.Evaluate(time);
                
                shockwaveObj.transform.localScale = maxShockwaveScale * shockwaveScaleCurve.Evaluate(time);

                yield return null;
                
                elapsedTime += Time.unscaledDeltaTime;
            }
            
            Destroy(shockwaveObj);
        }

        private IEnumerator EaseTimeScaleCoroutine(bool rightToLeft = false)
        {
            var elapsedTime = 0f;

            var spriteMaxScaleVector = Vector3.one * spriteMaxScale;

            while (elapsedTime < easeDuration)
            {
                var timeFromLeft = elapsedTime / easeDuration;

                var timeFromRight = 1f - timeFromLeft;

                var spriteScaleMultiplier = spriteScaleCurve.Evaluate(rightToLeft ? timeFromLeft : timeFromRight);

                transform.localScale = spriteMaxScaleVector * spriteScaleMultiplier;

                Time.timeScale = timeCurve.Evaluate(rightToLeft ? timeFromRight : timeFromLeft);;
                
                yield return null;
                
                elapsedTime += Time.unscaledDeltaTime;
            }
        }

        private IEnumerator RotateSpriteCoroutine()
        {
            while (true)
            {
                transform.Rotate(new Vector3(0f, 0f, 1f) * spriteRotateSpeed * Time.deltaTime);
                yield return null;
            }
        }

        private void HandleStaticTrigger(Vector2 position, float duration) => Activate(position, duration);
    }
}
