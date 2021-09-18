using RedBlueGames.NotNull;
using TMPro;
using UnityEngine;

namespace SIVS
{
    public class TextInputMenu : Menu
    {
        [NotNull]
        [Header("Text Input Menu")]
        [Tooltip("The label text component.")]
        public TMP_Text labelText;

        [NotNull]
        [Tooltip("The input field component.")]
        public TMP_InputField inputField;

        private ITextInputListener _listener;

        public void Configure(
            ITextInputListener listener,
            string label,
            string existingText = "",
            int maxLength = 16,
            TMP_InputField.ContentType contentType = TMP_InputField.ContentType.Standard,
            TMP_InputField.InputType inputType = TMP_InputField.InputType.Standard)
        {
            _listener = listener;
            labelText.text = label;
            inputField.text = existingText;
            inputField.characterLimit = maxLength;
            inputField.contentType = contentType;
            inputField.inputType = inputType;
        }

        public void Confirm()
        {
            _listener?.OnTextInputConfirm(inputField.text);
        }
    }
}