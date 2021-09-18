using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
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

        public UnityEvent onMenuHide;
        
        [Header("Set interactable if requirements are met:")]
        [Tooltip("If set, sets this menu as interactable after the bar canvas has opened.")]
        public BarCanvas barCanvasInteractableRequirement;

        protected CanvasGroup CanvasGroup;

        protected bool ShouldAlwaysBeInteractable;

        private Selectable _previouslySelectedSelectable;

        private EventSystem _eventSystem;

        private void Awake()
        {
            CanvasGroup = GetComponent<CanvasGroup>();
            _eventSystem = EventSystem.current;
        }

        protected virtual void OnEnable()
        {
            if (barCanvasInteractableRequirement)
                barCanvasInteractableRequirement.OnFinishOpen += HandleBarCanvasOpen;
        }

        protected virtual void OnDisable()
        {
            if (barCanvasInteractableRequirement)
                barCanvasInteractableRequirement.OnFinishOpen -= HandleBarCanvasOpen;
        }

        public virtual void Show()
        {
            ShowCanvasGroup(CanvasGroup);
            
            if (CanvasGroup.interactable)
                SelectPrimaryElement();
        }

        public void SelectPrimaryElement()
        {
            var automaticSelection = GetAutomaticSelection();

            if (_previouslySelectedSelectable && _previouslySelectedSelectable.interactable)
                _previouslySelectedSelectable.Select();
            else if (automaticSelection)
                automaticSelection.Select();
        }

        public virtual Selectable GetAutomaticSelection()
        {
            return autoSelect;
        }

        public void Hide()
        {
            onMenuHide.Invoke();
            
            HideCanvasGroup(CanvasGroup);

            var currentSelectedObj = _eventSystem.currentSelectedGameObject;

            if (currentSelectedObj)
                _previouslySelectedSelectable = currentSelectedObj.GetComponent<Selectable>();
            else
                _previouslySelectedSelectable = null;
        }

        protected void ShowCanvasGroup(CanvasGroup canvasGroup)
        {
            if (ShouldMakeInteractable())
                MakeCanvasGroupInteractable(canvasGroup);
            
            canvasGroup.alpha = 1.0f;
        }
        
        protected void MakeCanvasGroupInteractable(CanvasGroup canvasGroup)
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        public virtual void MakeInteractable()
        {
            MakeCanvasGroupInteractable(CanvasGroup);
        }
        
        protected void MakeCanvasGroupNonInteractable(CanvasGroup canvasGroup)
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
        
        public virtual void MakeNonInteractable()
        {
            MakeCanvasGroupNonInteractable(CanvasGroup);
        }

        protected void HideCanvasGroup(CanvasGroup canvasGroup)
        {
            MakeCanvasGroupNonInteractable(canvasGroup);
            
            canvasGroup.alpha = 0.0f;
        }

        protected virtual bool ShouldMakeInteractable()
        {
            if (ShouldAlwaysBeInteractable)
                return true;
            
            if (barCanvasInteractableRequirement)
                return false;

            return true;
        }
        
        private void HandleBarCanvasOpen()
        {
            MakeInteractableAndSelectPrimaryElement();
            MakeAlwaysInteractable();
        }

        protected void MakeInteractableAndSelectPrimaryElement()
        {
            MakeInteractable();
            SelectPrimaryElement();
        }

        protected void MakeAlwaysInteractable() =>
            ShouldAlwaysBeInteractable = true;
    }
}
