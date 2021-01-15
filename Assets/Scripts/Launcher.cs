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
        private const string GameVersion = "1.0";

        private bool _isConnecting;

        private void Awake()
        {
            Screen.SetResolution(1024, 768, false);
            Connect();
        }

        private void Connect()
        {
            if (PhotonNetwork.IsConnected) return;

            _isConnecting = PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = GameVersion;
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        public override void OnConnectedToMaster()
        {
            if (!_isConnecting) return;

            _isConnecting = false;
            SceneManager.LoadScene("MainMenu");
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            _isConnecting = false;
        }
    }
}
