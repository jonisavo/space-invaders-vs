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
        protected TMP_Dropdown _dropdown;

        private Coroutine _deselectCoroutine;

        protected override void Awake()
        {
            base.Awake();

            _dropdown = GetComponent<TMP_Dropdown>();

            var initialValues = GetInitialValues();
            
            _dropdown.AddOptions(initialValues.Options);
            
            _dropdown.SetValueWithoutNotify(initialValues.Index);
        }

        protected struct InitialDropdownValues
        {
            public List<TMP_Dropdown.OptionData> Options;
            public int Index;
        }

        protected virtual InitialDropdownValues GetInitialValues() =>
            new InitialDropdownValues 
            { 
                Options = _dropdown.options, 
                Index = 0 
            };

        public void OnPointerDown(PointerEventData eventData)
        {
            MenuManager.BlockGoingBack = _dropdown.interactable;
        }

        public override void OnPointerEnter(PointerEventData evt)
        {
            if (_dropdown.IsExpanded)
                return;

            base.OnPointerEnter(evt);
        }

        public override void OnPointerExit(PointerEventData evt)
        {
            if (_dropdown.IsExpanded)
                return;
            
            base.OnPointerExit(evt);
        }

        public override void OnSelect(BaseEventData evt)
        {
            if (_deselectCoroutine != null)
                ClearDeselectQueue();
            
            if (_dropdown.IsExpanded)
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
            yield return new WaitWhile(() => _dropdown.IsExpanded);

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
            MenuManager.BlockGoingBack = _dropdown.IsExpanded;
        }

        public virtual void OnCancel(BaseEventData evt)
        {
            MenuManager.BlockGoingBack = _dropdown.IsExpanded;
        }
    }
}