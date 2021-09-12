using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Random = UnityEngine.Random;

namespace SIVS
{
    [RequireComponent(typeof(InvaderManagerOnline))]
    public class GameManagerOnline : GameManager, IInRoomCallbacks, IMatchmakingCallbacks
    {
        #region Unity Callbacks

        protected override void Awake()
        {
            base.Awake();
            Random.InitState((int) DateTime.Now.Ticks + PhotonNetwork.LocalPlayer.ActorNumber);
        }
        
        protected override void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
            base.OnDisable();
        }

        private void Start()
        {
            PhotonNetwork.LocalPlayer.SetReady(true);
        }

        #endregion

        #region PUN Callbacks

        public void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps) { }

        public void OnLeftRoom()
        {
            SceneManager.LoadScene("MainMenu");
        }

        public void OnPlayerLeftRoom(Player otherPlayer)
        {
            if (GameOver) return;
            
            var otherPlayerNumber = otherPlayer.ActorNumber == 1 ? 2 : 1;

            EndGame(
                Players[otherPlayerNumber], 
                Players[otherPlayer.ActorNumber],
                VictoryReason.Leave
            );
        }

        public void OnJoinedRoom() { }

        public void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged) { }

        public void OnCreatedRoom() { }

        public void OnCreateRoomFailed(short returnCode, string message) { }

        public void OnFriendListUpdate(List<FriendInfo> friendList) { }

        public void OnJoinRandomFailed(short returnCode, string message) { }

        public void OnJoinRoomFailed(short returnCode, string message) { }

        public void OnMasterClientSwitched(Player newMasterClient) { }

        public void OnPlayerEnteredRoom(Player newPlayer) { }

        #endregion

        protected override void InitializePlayers()
        {
            Players[1] = new SIVSPhotonPlayer(PhotonNetwork.CurrentRoom.Players[1]);
            Players[2] = new SIVSPhotonPlayer(PhotonNetwork.CurrentRoom.Players[2]);
            
            Players[1].InitializeStats();
            Players[2].InitializeStats();
        }
        
        public override void LeaveGame()
        {
            PhotonNetwork.LeaveRoom();

            CleanupPlayers();
        }

        protected override void EndGame(SIVSPlayer winner, SIVSPlayer loser, VictoryReason victoryReason)
        {
            PhotonNetwork.LocalPlayer.SetReady(false);
            
            SetHighScore();
            
            base.EndGame(winner, loser, victoryReason);
        }

        private void SetHighScore()
        {
            var highScore = PlayerPrefs.GetInt("OnlineHighScore", 0);

            var score = PhotonNetwork.LocalPlayer.GetScore();

            if (score > highScore)
                PlayerPrefs.SetInt("OnlineHighScore", score);
        }
    }
}
