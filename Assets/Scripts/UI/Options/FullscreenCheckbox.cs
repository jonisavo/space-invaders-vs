using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SIVS
{
    [RequireComponent(typeof(Toggle))]
    public class FullscreenCheckbox : MonoBehaviour
    {
        private Toggle _toggle;

        private readonly UnityAction<bool> _toggleEvent = SetFullScreen;

        private void Awake()
        {
            _toggle = GetComponent<Toggle>();
            _toggle.isOn = Screen.fullScreen;
        }

        private void OnEnable() => _toggle.onValueChanged.AddListener(_toggleEvent);

        private void OnDisable() => _toggle.onValueChanged.RemoveListener(_toggleEvent);

        private static void SetFullScreen(bool value)
        {
            Screen.fullScreen = value;
            
            if (value)
                Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow);
            else
                Screen.SetResolution(1280, 720, FullScreenMode.Windowed);
        }
    }
}