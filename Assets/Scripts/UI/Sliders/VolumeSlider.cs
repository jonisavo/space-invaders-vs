using System;
using RedBlueGames.NotNull;
using UnityEngine;
using UnityEngine.Audio;

namespace SIVS
{
    public class VolumeSlider : MainMenuSlider
    {
        [Header("Volume Slider")]
        [NotNull]
        [Tooltip("Audio mixer to manipulate.")]
        public AudioMixer audioMixer;

        public enum Parameter
        {
            MusicVolume,
            SoundEffectVolume
        }

        [Tooltip("The parameter to manipulate.")]
        public Parameter parameter;
        
        protected override void Awake()
        {
            base.Awake();
            
            Slider.onValueChanged.AddListener(UpdateVolume);
        }

        private void Start() => UpdateVolume(Slider.value);

        protected override float GetInitialValue() =>
            PlayerPrefs.GetFloat(GetParameterName(), 0.8f);

        private void UpdateVolume(float value) =>
            audioMixer.SetFloat(GetParameterName(), Mathf.Log10(value) * 20);

        private string GetParameterName()
        {
            switch (parameter)
            {
                case Parameter.MusicVolume:
                    return PlayerPrefsKeys.MusicVolume;
                case Parameter.SoundEffectVolume:
                    return PlayerPrefsKeys.SoundEffectVolume;
            }

            throw new Exception("Unknown VolumeSlider parameter " + parameter);
        }
    }
}