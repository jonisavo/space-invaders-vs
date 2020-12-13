using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace SIVS
{
    public class Matchmaker : MonoBehaviourPunCallbacks
    {
        [Tooltip("The UI Panel to let the user enter name, connect and play")]
        public GameObject ControlPanel;

        [Tooltip("The UI Label to inform the user that the connection is in progress")]
        public GameObject ProgressLabel;

        private bool isMatchmaking;

        private void Awake()
        {
            isMatchmaking = false;
        }

        public void BeginMatchmaking()
        {
            if (string.IsNullOrEmpty(PhotonNetwork.NickName.Trim())) return;
            if (isMatchmaking) return;

            isMatchmaking = true;
            PhotonNetwork.JoinRandomRoom();
            ShowProgressLabel();
        }
        #region MonoBehaviourPunCallbacks Callbacks

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.LogWarningFormat("Matchmaker: OnDisconnected() called");

            if (!isMatchmaking) return;

            HideProgressLabel();
            isMatchmaking = false;
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("Matchmaker: OnJoinRandomFailed() called");

            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
        }

        public override void OnJoinedRoom()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.LoadLevel("InGame");
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            base.OnCreateRoomFailed(returnCode, message);
            isMatchmaking = false;
            HideProgressLabel();
        }

        #endregion

        private void ShowProgressLabel()
        {
            ControlPanel.SetActive(false);
            ProgressLabel.SetActive(true);
        }

        private void HideProgressLabel()
        {
            ControlPanel.SetActive(true);
            ProgressLabel.SetActive(false);
        }
    }
}
