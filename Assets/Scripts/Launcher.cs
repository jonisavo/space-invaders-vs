using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SIVS
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        /// <summary>
        /// This client's version number
        /// </summary>
        private const string GameVersion = "1";

        private bool _isConnecting;

        private void Awake()
        {
            Connect();
        }

        private void Connect()
        {
            if (PhotonNetwork.IsConnected) return;

            _isConnecting = PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = GameVersion;
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        #region MonoBehaviourPunCallbacks Callbacks

        public override void OnConnectedToMaster()
        {
            Debug.Log("OnConnectedToMaster() called");

            if (!_isConnecting) return;

            _isConnecting = false;
            SceneManager.LoadScene("MainMenu");
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.LogWarningFormat("OnDisconnected() called");
            _isConnecting = false;
        }

        #endregion
    }
}
