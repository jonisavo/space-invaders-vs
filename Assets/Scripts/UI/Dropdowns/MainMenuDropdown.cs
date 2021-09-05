using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SIVS
{
    [RequireComponent(typeof(TMP_Dropdown))]
    public class MainMenuDropdown : MainMenuSelectable, ISubmitHandler, ICancelHandler
    {
        protected TMP_Dropdown _dropdown;

        protected override void Awake()
        {
            base.Awake();

            _dropdown = GetComponent<TMP_Dropdown>();

            var initialValues = GetInitialValues();
            
            _dropdown.AddOptions(initialValues.options);
            
            _dropdown.SetValueWithoutNotify(initialValues.index);
        }

        protected struct InitialDropdownValues
        {
            public List<TMP_Dropdown.OptionData> options;
            public int index;
        }

        protected virtual InitialDropdownValues GetInitialValues() =>
            new InitialDropdownValues 
            { 
                options = _dropdown.options, 
                index = 0 
            };

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
            if (_dropdown.IsExpanded)
                return;
            
            base.OnSelect(evt);
        }

        public override void OnDeselect(BaseEventData evt)
        {
            if (_dropdown.IsExpanded)
                return;
            
            base.OnDeselect(evt);

            MenuManager.BlockGoingBack = false;
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