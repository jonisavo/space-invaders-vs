using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SIVS
{
    [RequireComponent(typeof(Selectable))]
    public class RainbowGradientSelectable : RainbowGradientMonoBehaviour
    {
        private Selectable _selectable;

        private EventSystem _eventSystem;

        private float _currentHue;

        private ColorBlock _currentColorBlock;

        private void Awake()
        {
            _selectable = GetComponent<Selectable>();
            _eventSystem = EventSystem.current;
            _currentColorBlock = _selectable.colors;
        }

        private void Update()
        {
            if (_eventSystem.currentSelectedGameObject != gameObject)
                return;

            AnimateSingleHue(ref _currentHue);
            
            _currentColorBlock.selectedColor = Color.HSVToRGB(_currentHue / 360f, 0.3f, 0.9f);

            _selectable.colors = _currentColorBlock;
        }
    }
}