using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace SIVS
{
    public class Matchmaker : MonoBehaviourPunCallbacks
    {
        [Tooltip("The UI Panel to let the user enter name, connect and play")]
        public GameObject controlPanel;

        [Tooltip("The UI Label to inform the user that the connection is in progress")]
        public GameObject progressLabel;

        [Tooltip("The Button to let the user start matchmaking quickly")]
        public Button quickPlayButton;

        [Tooltip("The Button to let the user join a room")]
        public Button joinRoomButton;

        [Tooltip("The Button to let the user cancel matchmaking")]
        public Button cancelButton;

        private bool _isMatchmaking;

        private bool _allowMatchmaking;

        private bool AllowMatchmaking
        {
            get => _allowMatchmaking;
            set
            {
                _allowMatchmaking = value;
                
                if (quickPlayButton)
                {
                    var shouldSelect = !quickPlayButton.interactable && value;

                    quickPlayButton.interactable = value;
                    
                    if (shouldSelect)
                        quickPlayButton.Select();
                }

                if (joinRoomButton)
                    joinRoomButton.interactable = value;
            }
        }

        private void Awake()
        {
            _isMatchmaking = false;
            AllowMatchmaking = PhotonNetwork.IsConnectedAndReady;
        }

        private void Update()
        {
            if (_isMatchmaking && Input.GetButtonDown("Cancel"))
                CancelMatchmaking();
        }

        public void BeginMatchmaking()
        {
            if (string.IsNullOrEmpty(PhotonNetwork.NickName.Trim())) return;
            if (_isMatchmaking || !AllowMatchmaking) return;

            _isMatchmaking = true;
            PhotonNetwork.JoinRandomRoom();
            ShowProgressLabel();
        }

        public void JoinNamedRoom(string roomName)
        {
            _isMatchmaking = true;
            PhotonNetwork.JoinOrCreateRoom(roomName,
                new RoomOptions
                {
                    MaxPlayers = 2, 
                    IsVisible = false, 
                    CustomRoomProperties = new Hashtable()
                    {
                        {"Active", false}
                    }
                },
                TypedLobby.Default
            );
            ShowProgressLabel();
        }

        public void CancelMatchmaking()
        {
            if (PhotonNetwork.InRoom)
                PhotonNetwork.LeaveRoom();

            StopMatchmaking();
            AllowMatchmaking = PhotonNetwork.IsConnectedAndReady;
        }

        private void StopMatchmaking()
        {
            _isMatchmaking = false;

            if (!controlPanel) return;
            
            HideCancelButton();
            HideProgressLabel();
        }

        #region PUN Callbacks

        public override void OnConnectedToMaster()
        {
            AllowMatchmaking = true;
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            AllowMatchmaking = false;

            if (!_isMatchmaking) return;

            StopMatchmaking();
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            PhotonNetwork.CreateRoom(null, new RoomOptions
            {
                MaxPlayers = 2,
                CustomRoomProperties = new Hashtable()
                {
                    { Match.ActivePropertyKey, false }
                }
            });
        }

        public override void OnJoinedRoom()
        {
            if (!_isMatchmaking)
            {
                PhotonNetwork.LeaveRoom();
                return;
            }

            if (!HasUniqueNickname())
            {
                CancelMatchmaking();
                return;
            }

            if (IsFullRoom())
                StartGame();
            else
                ShowCancelButton();
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            if (PhotonNetwork.LocalPlayer.NickName == newPlayer.NickName)
            {
                CancelMatchmaking();
                return;
            }

            if (IsFullRoom()) StartGame();
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            StopMatchmaking();
            AllowMatchmaking = PhotonNetwork.IsConnectedAndReady;
        }

        #endregion

        private bool IsFullRoom() =>
            PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers;

        private bool HasUniqueNickname()
        {
            foreach (var player in PhotonNetwork.CurrentRoom.Players.Values)
            {
                if (PhotonNetwork.LocalPlayer.ActorNumber == player.ActorNumber) continue;
                
                if (PhotonNetwork.LocalPlayer.NickName == player.NickName) return false;
            }

            return true;
        }

        private void StartGame()
        {
            if (!PhotonNetwork.IsMasterClient)
                return;

            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.LoadLevel("InGame");
        }

        private void ShowCancelButton()
        {
            cancelButton.gameObject.SetActive(true);
            cancelButton.Select();
        }

        private void HideCancelButton() =>
            cancelButton.gameObject.SetActive(false);
        
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
