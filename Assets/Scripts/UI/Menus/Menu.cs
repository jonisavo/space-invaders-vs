using UnityEngine;
using UnityEngine.UI;

namespace SIVS
{
    [RequireComponent(typeof(CanvasGroup))]
    public class Menu : MonoBehaviour
    {
        [Tooltip("A Selectable UI component to select automatically when showing this menu.")]
        public Selectable autoSelect;
        
        [Tooltip("Disables going back to the previous menu via the cancel button.")]
        public bool disableGoingBack;
        
        private CanvasGroup _canvasGroup;

        private void Awake() => _canvasGroup = GetComponent<CanvasGroup>();

        public void Show()
        {
            _canvasGroup.interactable = true;
            _canvasGroup.alpha = 1.0f;
            _canvasGroup.blocksRaycasts = true;
            
            if (autoSelect)
                autoSelect.Select();
        }

        public void Hide()
        {
            _canvasGroup.interactable = false;
            _canvasGroup.alpha = 0.0f;
            _canvasGroup.blocksRaycasts = false;
        }
    }
}