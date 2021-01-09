using UnityEngine;

namespace SIVS
{
    public class OptionsManager : MonoBehaviour
    {
        public GameObject optionsCanvas;

        private void Update()
        {
            if (Input.GetButtonDown("Cancel"))
                ToggleCanvas();
        }

        public void ToggleCanvas() =>
            optionsCanvas.SetActive(!optionsCanvas.activeInHierarchy);

        public void CloseCanvas() => optionsCanvas.SetActive(false);

        public bool IsCanvasActive() => optionsCanvas.activeInHierarchy;
    }
}
