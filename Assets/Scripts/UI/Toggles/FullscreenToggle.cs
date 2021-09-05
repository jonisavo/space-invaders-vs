using UnityEngine;

namespace SIVS
{
    public class FullscreenToggle : MainMenuToggle
    {
        protected override bool GetInitialValue()
        {
            var fallbackValue = Screen.fullScreen ? 1 : 0;

            return PlayerPrefs.GetInt(PlayerPrefsKeys.ScreenFullscreen, fallbackValue) == 1;
        }

        protected override void HandleValueChange(bool value)
        {
            base.HandleValueChange(value);
            
            PlayerPrefs.SetInt(PlayerPrefsKeys.ScreenFullscreen, value ? 1 : 0);

            Screen.fullScreenMode = value ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
            Screen.fullScreen = value;
        }
    }
}