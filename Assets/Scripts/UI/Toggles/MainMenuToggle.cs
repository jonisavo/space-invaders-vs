using System.Collections;
using RedBlueGames.NotNull;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SIVS
{
    [RequireComponent(typeof(Toggle))]
    public class MainMenuToggle : MainMenuSelectable
    {
        [Header("Toggle")]
        [NotNull]
        public TMP_Text onLabel;

        [NotNull]
        public TMP_Text offLabel;
        
        public Color enabledColor = Color.white;

        public Color disabledColor = Color.gray;

        private Toggle _toggle;

        private Coroutine _horizontalInputCoroutine;

        protected override void Awake()
        {
            base.Awake();
            
            _toggle = GetComponent<Toggle>();
            
            _toggle.onValueChanged.AddListener(HandleValueChange);

            _toggle.SetIsOnWithoutNotify(GetInitialValue());
            
            HandleValueChange(_toggle.isOn);
        }
        
        protected virtual bool GetInitialValue() => _toggle.isOn;

        protected virtual void HandleValueChange(bool value)
        {
            if (value)
            {
                onLabel.color = enabledColor;
                offLabel.color = disabledColor;
            }
            else
            {
                offLabel.color = enabledColor;
                onLabel.color = disabledColor;
            }
        }

        public override void EnableAllAnimation()
        {
            base.EnableAllAnimation();
            
            if (_horizontalInputCoroutine != null)
                StopCoroutine(_horizontalInputCoroutine);

            _horizontalInputCoroutine = StartCoroutine(CheckHorizontalInputCoroutine());
        }

        public override void DisableAllAnimation()
        {
            base.DisableAllAnimation();

            if (_horizontalInputCoroutine == null)
                return;
            
            StopCoroutine(_horizontalInputCoroutine);

            _horizontalInputCoroutine = null;
        }

        private IEnumerator CheckHorizontalInputCoroutine()
        {
            var cooldownBetweenToggles = new WaitForSeconds(0.3f);
            
            while (true)
            {
                yield return null;

                var change = Input.GetAxisRaw("Horizontal") != 0;

                if (!change)
                    continue;
                
                _toggle.isOn = !_toggle.isOn;

                yield return cooldownBetweenToggles;
            }
        }
    }
}