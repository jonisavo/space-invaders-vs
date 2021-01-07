﻿using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace SIVS
{
    [RequireComponent(typeof(UIManager))]
    [RequireComponent(typeof(InvaderManager))]
    public class GameManager : MonoBehaviourPunCallbacks
    {
        [Tooltip("Audio clip to play upon a victory.")]
        public AudioClip victorySound;
        
        private bool _bothReady = false;

        private bool _gameOver = false;

        private UIManager _uiManager;

        private InvaderManager _invaderManager;
        
        #region Unity Callbacks
        
        private void Awake()
        {
            PlayerStats.InitializeStats(PhotonNetwork.LocalPlayer);
            _uiManager = GetComponent<UIManager>();
            _invaderManager = GetComponent<InvaderManager>();
        }

        #endregion

        #region PUN Callbacks

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if (_gameOver) return;
            
            if (changedProps.ContainsKey(PlayerStats.Ready))
            {
                if (!_bothReady && IsEveryoneReady())
                {
                    _bothReady = true;
                    
                    _invaderManager.InitializeInvaders();

                    if (PhotonNetwork.IsMasterClient)
                        Match.IsActive = true;
                }
            }

            if (changedProps.ContainsKey(PlayerStats.Lives))
            {
                if ((int) changedProps[PlayerStats.Lives] <= 0)
                    EndGame(GetOtherPlayer(targetPlayer));
            }

            if (!changedProps.ContainsKey(PlayerStats.CurrentRound))
                return;
            
            if ((int) changedProps[PlayerStats.CurrentRound] >= 6)
                EndGame(targetPlayer);
        }

        public override void OnLeftRoom()
        {
            SceneManager.LoadScene("MainMenu");
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            if (_gameOver) return;
            
            EndGame(GetOtherPlayer(otherPlayer));
        }

        #endregion

        public void LeaveGame() => PhotonNetwork.LeaveRoom();

        private bool IsEveryoneReady()
        {
            foreach (var player in PhotonNetwork.CurrentRoom.Players.Values)
            {
                if (player == null) return false;

                if (!player.CustomProperties.ContainsKey(PlayerStats.Ready))
                    return false;

                if (!(bool)player.CustomProperties[PlayerStats.Ready])
                    return false;
            }
            
            return true;
        }
        
        private Player GetOtherPlayer(Player firstPlayer)
        {
            foreach (var player in PhotonNetwork.CurrentRoom.Players.Values)
                if (player.ActorNumber != firstPlayer.ActorNumber)
                    return player;

            return null;
        }

        private void EndGame(Player winner)
        {
            _gameOver = true;

            if (PhotonNetwork.IsMasterClient)
                Match.IsActive = false;
            
            _uiManager.ShowVictoryScreen(winner == null ? "No one" : winner.NickName);

            GameObject.Find("Music Player")
                .GetComponent<AudioSource>()
                .Stop();
            
            if (winner != null && winner.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
                GameObject.Find("Sound Player")
                    .GetComponent<AudioSource>()
                    .PlayOneShot(victorySound);

            StopAllCoroutines();
            
            DestroyBullets();

            _invaderManager.StopAllCoroutines();
            
            foreach (var invader in GameObject.FindGameObjectsWithTag("Invader"))
                invader.GetComponent<InvaderShoot>().StopShooting();
        }

        private void DestroyBullets()
        {
            foreach (var playerBullet in GameObject.FindGameObjectsWithTag("PlayerBullet"))
                Destroy(playerBullet);

            foreach (var enemyBullet in GameObject.FindGameObjectsWithTag("EnemyBullet"))
                Destroy(enemyBullet);
        }
    }
}
