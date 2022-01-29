using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using RedBlueGames.NotNull;

namespace SIVS
{
    public class OptionsManager : MonoBehaviourPunCallbacks
    {
        [NotNull]
        public GameObject optionsCanvas;

        [NotNull]
        public Button closeButton;

        public delegate void OptionsOpenDelegate();

        public delegate void OptionsCloseDelegate();

        public static event OptionsOpenDelegate OnOptionsOpen;

        public static event OptionsCloseDelegate OnOptionsClose;

        private bool _allowOpeningOptions = true;

        public override void OnEnable()
        {
            base.OnEnable();

            GameManager.OnGameEnd += OnGameEnd;
        }

        public override void OnDisable()
        {
            base.OnDisable();

            GameManager.OnGameEnd -= OnGameEnd;
        }

        private void Update()
        {
            if (!_allowOpeningOptions && !IsCanvasActive())
                return;
            
            if (Input.GetButtonDown("Cancel"))
                ToggleCanvas();
        }

        private void OnGameEnd(SIVSPlayer winner, SIVSPlayer loser, VictoryReason victoryReason)
        {
            CloseCanvas();
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
