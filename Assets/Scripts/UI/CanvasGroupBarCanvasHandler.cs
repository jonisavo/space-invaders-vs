using RedBlueGames.NotNull;
using UnityEngine;

namespace SIVS
{
    [RequireComponent(typeof(CanvasGroup))]
    public class CanvasGroupBarCanvasHandler : MonoBehaviour
    {
        [NotNull (IgnorePrefab = true)]
        [Tooltip("The bar canvas to react to.")]
        public BarCanvas barCanvas;

        public enum HandleMethod
        {
            Ignore,
            EnableInteractable,
            DisableInteractable
        }

        [Tooltip("Determines how the Canvas Group reacts to bars opening.")]
        public HandleMethod handleOpen;

        [Tooltip("Determines how the Canvas Group reacts to bars closing.")]
        public HandleMethod handleClose;

        private CanvasGroup _canvasGroup;

        private void Awake() => _canvasGroup = GetComponent<CanvasGroup>();

        private void OnEnable()
        {
            if (handleOpen != HandleMethod.Ignore)
                barCanvas.OnFinishOpen += HandleCanvasGroupOpen;

            if (handleClose != HandleMethod.Ignore)
                barCanvas.OnFinishClose += HandleCanvasGroupClose;
        }

        private void OnDisable()
        {
            barCanvas.OnFinishOpen -= HandleCanvasGroupOpen;
            barCanvas.OnFinishClose -= HandleCanvasGroupClose;
        }

        private void HandleCanvasGroupOpen()
        {
            _canvasGroup.interactable = handleOpen switch
            {
                HandleMethod.EnableInteractable => true,
                HandleMethod.DisableInteractable => false,
                _ => _canvasGroup.interactable
            };
        }

        private void HandleCanvasGroupClose()
        {
            _canvasGroup.interactable = handleClose switch
            {
                HandleMethod.EnableInteractable => true,
                HandleMethod.DisableInteractable => false,
                _ => _canvasGroup.interactable
            };
        }
    }
}