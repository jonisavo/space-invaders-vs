using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace SIVS
{
    public class Matchmaker : MonoBehaviourPunCallbacks
    {
        [Tooltip("The UI Panel to let the user enter name, connect and play")]
        public GameObject controlPanel;

        [Tooltip("The UI Label to inform the user that the connection is in progress")]
        public GameObject progressLabel;

        private bool _isMatchmaking;

        private void Awake()
        {
            _isMatchmaking = false;
        }

        public void BeginMatchmaking()
        {
            if (string.IsNullOrEmpty(PhotonNetwork.NickName.Trim())) return;
            if (_isMatchmaking) return;

            _isMatchmaking = true;
            PhotonNetwork.JoinRandomRoom();
            ShowProgressLabel();
        }
        #region MonoBehaviourPunCallbacks Callbacks

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.LogWarningFormat("Matchmaker: OnDisconnected() called");

            if (!_isMatchmaking) return;

            HideProgressLabel();
            _isMatchmaking = false;
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("Matchmaker: OnJoinRandomFailed() called");

            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
        }

        public override void OnJoinedRoom()
        {
            PhotonNetwork.LoadLevel("InGame");
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            base.OnCreateRoomFailed(returnCode, message);
            _isMatchmaking = false;
            HideProgressLabel();
        }

        #endregion

        private void ShowProgressLabel()
        {
            controlPanel.SetActive(false);
            progressLabel.SetActive(true);
        }

        private void HideProgressLabel()
        {
            controlPanel.SetActive(true);
            progressLabel.SetActive(false);
        }
    }
}
