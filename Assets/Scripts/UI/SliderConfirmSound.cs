using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SIVS
{
    [RequireComponent(typeof(Slider))]
    [RequireComponent(typeof(AudioSource))]
    public class SliderConfirmSound : MonoBehaviour, IPointerUpHandler
    {
        private Slider _slider;

        private AudioSource _audioSource;

        private float _oldValue;

        private void Awake()
        {
            _slider = GetComponent<Slider>();
            _audioSource = GetComponent<AudioSource>();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_slider.value != _oldValue)
                _audioSource.Play();
        }
    }
}