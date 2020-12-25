﻿using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Random = UnityEngine.Random;

namespace SIVS
{
    [RequireComponent(typeof(UIManager))]
    public class GameManager : MonoBehaviourPunCallbacks
    {
        private int _totalInvaderKills = 0;

        private bool _bothReady = false;

        private UIManager _uiManager;
        
        #region MonoBehaviour Callbacks
        
        private void Awake()
        {
            PlayerStats.InitializeStats();
            _uiManager = GetComponent<UIManager>();
        }
        
        #endregion
        
        #region MonoBehaviourPunCallbacks Callbacks

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if (changedProps.ContainsKey(PlayerStats.Ready))
            {
                if (!_bothReady && IsEveryoneReady())
                {
                    _bothReady = true;
                    if (PhotonNetwork.IsMasterClient)
                        SpawnInvadersForAll();
                }
            }

            if (changedProps.ContainsKey(PlayerStats.Lives))
            {
                if ((int) changedProps[PlayerStats.Lives] <= 0)
                    EndGame(GetOtherPlayer(targetPlayer));
            }

            if (changedProps.ContainsKey(PlayerStats.CurrentRound))
            {
                if ((int) changedProps[PlayerStats.CurrentRound] >= 6)
                    EndGame(targetPlayer);
            }

            if (!changedProps.ContainsKey(PlayerStats.InvaderKills)) return;
            
            var invaderKills = 0;
            
            foreach (var player in PhotonNetwork.CurrentRoom.Players.Values)
                if (player.CustomProperties.ContainsKey(PlayerStats.InvaderKills))
                    invaderKills += (int) player.CustomProperties[PlayerStats.InvaderKills];

            _totalInvaderKills = invaderKills;
        }
        
        #endregion
        
        private Player GetOtherPlayer(Player firstPlayer)
        {
            foreach (var player in PhotonNetwork.CurrentRoom.Players.Values)
                if (player.ActorNumber != firstPlayer.ActorNumber)
                    return player;

            return null;
        }

        private void EndGame(Player winner)
        {
            if (winner == null)
                _uiManager.ShowVictoryScreen("No one");
            else
                _uiManager.ShowVictoryScreen(winner.NickName);
            
            StopAllCoroutines();
        }

        private bool IsEveryoneReady()
        {
            foreach (var player in PhotonNetwork.CurrentRoom.Players.Values)
            {
                if (player == null) return false;

                if (!player.CustomProperties.ContainsKey(PlayerStats.Ready))
                    return false;

                if (!(bool)player.CustomProperties[PlayerStats.Ready]) return false;
            }
            
            return true;
        }

        private void SpawnInvadersForAll()
        {
            foreach (var player in PhotonNetwork.CurrentRoom.Players.Values)
                SpawnInvaders(player);
        }

        private void SpawnInvaders(Player player)
        {
            var rows = 4 + (int) player.CustomProperties[PlayerStats.CurrentRound];
            var columns = 4 + (int) player.CustomProperties[PlayerStats.CurrentRound] / 2;
            
            for (var row = 0; row < rows; row++)
                for (var column = 0; column < columns; column++)
                    SpawnOneInvader(player.ActorNumber, row, column);
        }

        private void SpawnOneInvader(int side, int row, int column)
        {
            object[] instantiationData = {GenerateInvaderHealth(), Random.Range(180, 300)};

            var xPos = (side == 1 ? -4.3f : 0.8f) + row * 0.4f;

            var yPos = 2.1f - column * 0.3f;

            PhotonNetwork.InstantiateRoomObject("Invader", new Vector3(xPos, yPos, 0),
                Quaternion.identity, 0, instantiationData);
        }

        private int GenerateInvaderHealth()
        {
            int invaderHealth;

            if (_totalInvaderKills < 15)
                invaderHealth = 1;
            else if (_totalInvaderKills < 35)
                invaderHealth = Random.Range(1, 4);
            else if (_totalInvaderKills < 60)
                invaderHealth = Random.Range(2, 6);
            else
                invaderHealth = Random.Range(2, 10);

            return invaderHealth;
        }
    }
}