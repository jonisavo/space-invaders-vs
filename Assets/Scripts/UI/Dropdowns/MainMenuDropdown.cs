using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SIVS
{
    [RequireComponent(typeof(TMP_Dropdown))]
    public class MainMenuDropdown : MainMenuSelectable, ISubmitHandler, ICancelHandler, IPointerDownHandler
    {
        protected TMP_Dropdown Dropdown;

        private Coroutine _deselectCoroutine;

        protected override void Awake()
        {
            base.Awake();

            Dropdown = GetComponent<TMP_Dropdown>();

            var initialValues = GetInitialValues();
            
            Dropdown.AddOptions(initialValues.Options);
            
            Dropdown.SetValueWithoutNotify(initialValues.Index);
        }

        protected struct InitialDropdownValues
        {
            public List<TMP_Dropdown.OptionData> Options;
            public int Index;
        }

        protected virtual InitialDropdownValues GetInitialValues() =>
            new InitialDropdownValues 
            { 
                Options = Dropdown.options, 
                Index = 0 
            };

        public void OnPointerDown(PointerEventData eventData)
        {
            MenuManager.BlockGoingBack = Dropdown.interactable;
        }

        public override void OnPointerEnter(PointerEventData evt)
        {
            if (Dropdown.IsExpanded)
                return;

            base.OnPointerEnter(evt);
        }

        public override void OnPointerExit(PointerEventData evt)
        {
            if (Dropdown.IsExpanded)
                return;
            
            base.OnPointerExit(evt);
        }

        public override void OnSelect(BaseEventData evt)
        {
            if (_deselectCoroutine != null)
                ClearDeselectQueue();
            
            if (Dropdown.IsExpanded)
                return;

            base.OnSelect(evt);
        }

        public override void OnDeselect(BaseEventData evt)
        {
            QueueDropdownToBeDeselected(evt);
        }
        
        private void QueueDropdownToBeDeselected(BaseEventData evt)
        {
            if (_deselectCoroutine != null)
                StopCoroutine(_deselectCoroutine);
            
            _deselectCoroutine = StartCoroutine(DeselectWhenDropdownInactive(evt));
        }
        
        private IEnumerator DeselectWhenDropdownInactive(BaseEventData evt)
        {
            yield return new WaitWhile(() => Dropdown.IsExpanded);

            base.OnDeselect(evt);

            MenuManager.BlockGoingBack = false;

            _deselectCoroutine = null;
        }

        private void ClearDeselectQueue()
        {
            StopCoroutine(_deselectCoroutine);
            _deselectCoroutine = null;
        }

        public virtual void OnSubmit(BaseEventData evt)
        {
            MenuManager.BlockGoingBack = Dropdown.IsExpanded;
        }

        public virtual void OnCancel(BaseEventData evt)
        {
            MenuManager.BlockGoingBack = Dropdown.IsExpanded;
        }
    }
}