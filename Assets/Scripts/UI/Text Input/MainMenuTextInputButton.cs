using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SIVS
{
    [RequireComponent(typeof(Button))]
    public class MainMenuTextInputButton : MainMenuSelectable, ITextInputListener
    {
        [Header("Text Input Button")]
        [Tooltip("The header to show in the text input screen.")]
        public string label;
        
        [Tooltip("A text component to write the input into, if set.")]
        public TMP_Text writtenTextLabel;

        [Tooltip("If set, shows this label if given input is invalid.")]
        public TMP_Text invalidInputLabel;

        private TextInputMenu _textInputMenu;

        private Button _button;

        protected override void Awake()
        {
            base.Awake();

            _button = GetComponent<Button>();
            
            _textInputMenu = FindObjectOfType<TextInputMenu>();
            
            if (!_textInputMenu)
            {
                Debug.LogError("A Text Input Canvas could not be found!", this);
                Destroy(gameObject);
            }
        }

        protected virtual void OnEnable()
        {
            _button.onClick.AddListener(ShowInputScreen);
        }

        protected virtual void OnDisable()
        {
            _button.onClick.RemoveListener(ShowInputScreen);
        }

        private void ShowInputScreen()
        {
            _textInputMenu.Configure(this, label, GetExistingText(), 12);
            MenuManager.PushMenu("TextInput");
        }

        protected virtual string GetExistingText() => "";

        protected virtual bool IsInputValid(string input) =>
            !string.IsNullOrEmpty(input.Trim());

        public virtual void OnTextInputConfirm(string input)
        {
            if (!IsInputValid(input))
                return;

            if (writtenTextLabel)
                writtenTextLabel.text = input;

            DetermineShownLabel(input);
        }

        protected void DetermineShownLabel(string input)
        {
            if (!writtenTextLabel || !invalidInputLabel)
                return;
            
            var inputValid = IsInputValid(input);

            writtenTextLabel.enabled = inputValid;
            invalidInputLabel.enabled = !inputValid;
        }
    }
}