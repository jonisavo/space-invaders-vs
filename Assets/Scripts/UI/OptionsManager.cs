using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

namespace SIVS
{
    public class OptionsManager : MonoBehaviourPunCallbacks
    {
        public GameObject optionsCanvas;

        public Button closeButton;

        private bool _allowOpeningOptions = true;

        private void Update()
        {
            if (!_allowOpeningOptions && !IsCanvasActive())
                return;
            
            if (Input.GetButtonDown("Cancel"))
                ToggleCanvas();
        }

        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            if (propertiesThatChanged.ContainsKey(Match.ActivePropertyKey))
                _allowOpeningOptions = (bool) propertiesThatChanged[Match.ActivePropertyKey];
        }

        public void ToggleCanvas()
        {
            optionsCanvas.SetActive(!optionsCanvas.activeInHierarchy);
            
            if (optionsCanvas.activeInHierarchy)
                closeButton.Select();
        }

        public void CloseCanvas() => optionsCanvas.SetActive(false);

        public bool IsCanvasActive() => optionsCanvas.activeInHierarchy;
    }
}
