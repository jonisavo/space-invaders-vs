using System.Collections;
using RedBlueGames.Tools.TextTyper;
using UnityEngine;

namespace SIVS
{
    [RequireComponent(typeof(TextTyper))]
    public class TextPopup : MonoBehaviour
    {
        [TextArea]
        public string text;

        [Range(0.5f, 10f)]
        public float duration = 1.0f;

        [Range(0, 2f)]
        public float typeDelay = 0.02f;

        public float untypeDelay = -1;

        public bool activateAutomatically;

        public delegate void SpawnDelegate(TextPopup textPopup);

        public static event SpawnDelegate OnSpawn;

        private TextTyper _textTyper;

        private void Awake()
        {
            _textTyper = GetComponent<TextTyper>();
            OnSpawn?.Invoke(this);
        }

        private void OnEnable()
        {
            _textTyper.PrintCompleted.AddListener(() => StartCoroutine(nameof(Hide)));

            if (activateAutomatically)
                Show();
        }

        private void OnDisable()
        {
            _textTyper.PrintCompleted.RemoveListener(() => StartCoroutine(nameof(Hide)));
        }

        public void Show()
        {
            _textTyper.TypeText(text, typeDelay);
        }

        public void Show(string textToShow)
        {
            _textTyper.TypeText(textToShow, typeDelay);
        }

        public void ChangeText(string newText) => text = newText;

        private IEnumerator Hide()
        {
            yield return new WaitForSeconds(duration);
            
            yield return _textTyper.UntypeTextCharByChar(untypeDelay);
            
            Destroy(gameObject);
        }
    }
}