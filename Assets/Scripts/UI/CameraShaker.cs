using System.Collections;
using UnityEngine;

namespace SIVS
{
    [RequireComponent(typeof(Camera))]
    // Based on the work of Jordan Kisiel:
    // http://jordankisiel.com/writing/screen_shake.php
    // https://github.com/JordanKisiel/UnityCameraShake/
    public class CameraShaker : MonoBehaviour
    {
        [Min(-1)]
        public int id;

        private delegate void StartShakingDelegate(int id, float amplitude, float duration, float damping);

        private static event StartShakingDelegate OnStartShaking;

        private float _currentShakeAmplitude = -1f;

        public void OnEnable() => OnStartShaking += HandleStartShaking;

        public void OnDisable() => OnStartShaking -= HandleStartShaking;

        public void Shake(float amplitude, float duration, float damping = 0.8f)
        {
            if (amplitude < _currentShakeAmplitude)
                return;
            
            var clampedDamping = Mathf.Clamp(damping, 0.0f, 1.0f);
            
            StopCoroutine(nameof(ShakeCoroutine));
            StartCoroutine(ShakeCoroutine(amplitude, duration, clampedDamping));
        }

        public static void ShakeById(int id, float amplitude, float duration, float damping = 0.8f) =>
            OnStartShaking?.Invoke(id, amplitude, duration, damping);

        public static void ShakeAll(float amplitude, float duration, float damping = 0.8f) =>
            ShakeById(-1, amplitude, duration, damping);

        private IEnumerator ShakeCoroutine(float amplitude, float duration, float damping)
        {
            _currentShakeAmplitude = amplitude;

            var originalPosition = transform.position;

            var elapsedTime = 0.0f;
            var currentDamping = 1.0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                
                var percentComplete = elapsedTime / duration;

                if (percentComplete >= damping && percentComplete <= 1.0f)
                    currentDamping = 1.0f - percentComplete;

                var offsetValues = Random.insideUnitCircle;

                offsetValues *= amplitude * currentDamping;

                transform.position = new Vector3(offsetValues.x, offsetValues.y, originalPosition.z);

                yield return null;
            }

            transform.position = originalPosition;

            _currentShakeAmplitude = -1f;
        }

        private void HandleStartShaking(int idToShake, float amplitude, float duration, float damping)
        {
            if (idToShake != -1 && idToShake != id)
                return;
            
            Shake(amplitude, duration, damping);
        }
    }
}