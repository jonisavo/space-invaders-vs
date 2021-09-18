using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using RedBlueGames.NotNull;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace SIVS
{
    public class Matchmaker : MonoBehaviourPunCallbacks
    {
        [Tooltip("The Menu Manager to interface with.")]
        [NotNull]
        public MenuManager menuManager;

        [Tooltip("The name of the menu to open when matchmaking.")]
        public string matchmakingMenuName;

        private bool _isMatchmaking;

        private bool _allowMatchmaking;

        private void Awake()
        {
            _isMatchmaking = false;
            _allowMatchmaking = PhotonNetwork.IsConnectedAndReady;
        }

        private void Update()
        {
            if (_isMatchmaking && Input.GetButtonDown("Cancel"))
                CancelMatchmaking();
        }

        public void BeginMatchmaking()
        {
            if (string.IsNullOrEmpty(PhotonNetwork.NickName.Trim()))
                return;
            
            if (_isMatchmaking || !_allowMatchmaking)
                return;

            _isMatchmaking = true;

            PhotonNetwork.JoinRandomRoom();
            menuManager.Push(matchmakingMenuName);
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
                        { Match.ActivePropertyKey, false }
                    }
                },
                TypedLobby.Default
            );
            menuManager.Push(matchmakingMenuName);
        }

        public void CancelMatchmaking()
        {
            if (PhotonNetwork.InRoom)
                PhotonNetwork.LeaveRoom();

            _isMatchmaking = false;
            _allowMatchmaking = PhotonNetwork.IsConnectedAndReady;
        }

        #region PUN Callbacks

        public override void OnConnectedToMaster()
        {
            _allowMatchmaking = true;
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            _allowMatchmaking = false;

            _isMatchmaking = false;
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            PhotonNetwork.CreateRoom(null, new RoomOptions
            {
                MaxPlayers = 2,
                CustomRoomProperties = new Hashtable
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
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            if (PhotonNetwork.LocalPlayer.NickName == newPlayer.NickName)
            {
                CancelMatchmaking();
                return;
            }

            if (IsFullRoom())
                StartGame();
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            _isMatchmaking = false;
            _allowMatchmaking = PhotonNetwork.IsConnectedAndReady;
        }

        #endregion

        private bool IsFullRoom() =>
            PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers;

        private bool HasUniqueNickname()
        {
            foreach (var player in PhotonNetwork.CurrentRoom.Players.Values)
            {
                if (PhotonNetwork.LocalPlayer.ActorNumber == player.ActorNumber)
                    continue;
                
                if (PhotonNetwork.LocalPlayer.NickName == player.NickName)
                    return false;
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
    }
}
