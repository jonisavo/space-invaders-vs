using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace SIVS
{
    public class Matchmaker : MonoBehaviourPunCallbacks
    {
        [Tooltip("The UI Panel to let the user enter name, connect and play")]
        [SerializeField]
        private GameObject controlPanel;

        [Tooltip("The UI Label to inform the user that the connection is in progress")]
        [SerializeField]
        private GameObject progressLabel;

        private bool isMatchmaking;

        public void BeginMatchmaking()
        {
            if (string.IsNullOrEmpty(PhotonNetwork.NickName.Trim())) return;

            PhotonNetwork.JoinRandomRoom();
            ShowProgressLabel();
        }
        #region MonoBehaviourPunCallbacks Callbacks

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.LogWarningFormat("Matchmaker: OnDisconnected() called");
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
