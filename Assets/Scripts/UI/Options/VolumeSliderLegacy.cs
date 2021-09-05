using RedBlueGames.NotNull;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace SIVS
{
    [RequireComponent(typeof(Slider))]
    public class VolumeSliderLegacy : MonoBehaviour
    {
        [NotNull]
        public AudioMixer mixer;

        public string parameterName;

        private void Awake()
        {
            GetComponent<Slider>().value = 
                PlayerPrefs.GetFloat(parameterName, 0.8f);
        }

        public void SetVolume(float sliderValue)
        {
            ChangeVolume(sliderValue);
            PlayerPrefs.SetFloat(parameterName, sliderValue);
        }

        private void ChangeVolume(float sliderValue) =>
            mixer.SetFloat(parameterName, Mathf.Log10(sliderValue) * 20);
    }
}