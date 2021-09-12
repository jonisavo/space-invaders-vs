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
        
        protected Slider Slider;
        
        protected override void Awake()
        {
            base.Awake();

            Slider = GetComponent<Slider>();
            
            Slider.onValueChanged.AddListener(SetValueLabel);

            Slider.value = GetInitialValue();
            
            SetValueLabel(Slider.value);
        }

        protected virtual float GetInitialValue() => Slider.value;

        private void SetValueLabel(float value)
        {
            valueLabel.text = string.Format(valueFormatString, value);
        }
    }
}