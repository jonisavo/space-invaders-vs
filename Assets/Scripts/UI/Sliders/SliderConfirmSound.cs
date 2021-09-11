using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SIVS
{
    [RequireComponent(typeof(Slider))]
    [RequireComponent(typeof(AudioSource))]
    public class SliderConfirmSound : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private Slider _slider;

        private AudioSource _audioSource;

        private Coroutine _confirmSoundCoroutine;

        private bool _pointerDown;

        private float _oldValue;

        private void Awake()
        {
            _slider = GetComponent<Slider>();
            _audioSource = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            _slider.onValueChanged.AddListener(HandleValueChanged);
        }

        private void OnDisable()
        {
            _slider.onValueChanged.RemoveListener(HandleValueChanged);
        }

        private void HandleValueChanged(float value)
        {
            if (Mathf.Approximately(value, _oldValue))
                return;
            
            if (_confirmSoundCoroutine != null)
                StopCoroutine(_confirmSoundCoroutine);

            _confirmSoundCoroutine = StartCoroutine(PlayConfirmSoundCoroutine());
        }

        private IEnumerator PlayConfirmSoundCoroutine()
        {
            yield return new WaitForSeconds(0.05f);
            
            if (!_pointerDown)
            {
                _audioSource.Play();
                _oldValue = _slider.value;
            }

            _confirmSoundCoroutine = null;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _pointerDown = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _pointerDown = false;
            
            HandleValueChanged(_slider.value);
        }
    }
}