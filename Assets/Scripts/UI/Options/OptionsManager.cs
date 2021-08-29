using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using RedBlueGames.NotNull;

namespace SIVS
{
    public class OptionsManager : MonoBehaviourPunCallbacks
    {
        [NotNull (IgnorePrefab = true)]
        public GameObject optionsCanvas;

        [NotNull (IgnorePrefab = true)]
        public Button closeButton;

        public delegate void OptionsOpenDelegate();

        public delegate void OptionsCloseDelegate();

        public static event OptionsOpenDelegate OnOptionsOpen;

        public static event OptionsCloseDelegate OnOptionsClose;

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
            if (optionsCanvas.activeInHierarchy)
                CloseCanvas();
            else
                OpenCanvas();
        }

        public void OpenCanvas()
        {
            optionsCanvas.SetActive(true);
            OnOptionsOpen?.Invoke();
            closeButton.Select();
        }

        public void CloseCanvas()
        {
            optionsCanvas.SetActive(false);
            OnOptionsClose?.Invoke();
        }

        public bool IsCanvasActive() => optionsCanvas.activeInHierarchy;
    }
}
