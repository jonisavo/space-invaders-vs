using RedBlueGames.NotNull;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace SIVS
{
    [RequireComponent(typeof(Slider))]
    public class MainMenuSlider : MainMenuSelectable
    {
        [Header("Slider")]
        [NotNull]
        [Tooltip("The value label component to set when the slider updates.")]
        public TMP_Text valueLabel;

        [FormerlySerializedAs("formatString")]
        [Tooltip("The format used for the value label, e.g. {0:#0%} or just {0}.")]
        public string valueFormatString = "{0}";
        
        protected Slider _slider;
        
        protected override void Awake()
        {
            base.Awake();

            _slider = GetComponent<Slider>();
            
            _slider.onValueChanged.AddListener(SetValueLabel);

            _slider.value = GetInitialValue();
            
            SetValueLabel(_slider.value);
        }

        protected virtual float GetInitialValue() => _slider.value;

        private void SetValueLabel(float value)
        {
            valueLabel.text = string.Format(valueFormatString, value);
        }
    }
}