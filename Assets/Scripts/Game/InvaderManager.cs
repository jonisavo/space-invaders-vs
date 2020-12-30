﻿using System.Collections;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Random = UnityEngine.Random;

namespace SIVS
{
    public class InvaderManager : MonoBehaviourPunCallbacks
    {
        private int _totalInvaderKills = 0;

        #region Callbacks

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if (!changedProps.ContainsKey(PlayerStats.InvaderKills)) return;
            
            var invaderKills = 0;
            
            foreach (var player in PhotonNetwork.CurrentRoom.Players.Values)
                if (player.CustomProperties.ContainsKey(PlayerStats.InvaderKills))
                    invaderKills += (int) player.CustomProperties[PlayerStats.InvaderKills];

            _totalInvaderKills = invaderKills;
        }
        
        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
                StartCoroutines();
        }

        #endregion
        
        #region Coroutines
    
        private IEnumerator MoveInvaders(Player player)
        {
            var side = player.ActorNumber;
            
            while (true)
            {
                yield return new WaitForSeconds(GetMoveInterval(player));

                var invader = GetInvaderFromSide(side);

                if (!invader) continue;

                var movement = invader.GetComponent<InvaderMovement>();

                if (movement.CanMoveInDirection(movement.GetMovementDirection(), 0.5f))
                {
                    MoveInvadersInSide(side, movement.GetMovementDirection());
                }
                else if (movement.CanMoveInDirection(Vector2.down, 2.5f))
                {
                    MoveInvadersInSide(side, Vector2.down);
                    TurnAroundInvadersInSide(side);
                }
            }
        }

        #endregion
        
        public void StartCoroutines()
        {
            foreach (var player in PhotonNetwork.CurrentRoom.Players.Values)
                StartCoroutine(MoveInvaders(player));
        }
        
        public void SpawnInvadersForAll()
        {
            foreach (var player in PhotonNetwork.CurrentRoom.Players.Values)
                SpawnInvaders(player);
        }
    
        public void SpawnInvaders(Player player)
        {
            var rows = 4 + (int) player.CustomProperties[PlayerStats.CurrentRound];
            var columns = 4 + (int) player.CustomProperties[PlayerStats.CurrentRound] / 2;
                
            for (var row = 0; row < rows; row++)
                for (var column = 0; column < columns; column++)
                    SpawnOneInvader(player.ActorNumber, row, column);
        }
    
        private void SpawnOneInvader(int side, int row, int column)
        {
            object[] instantiationData = {side, GenerateInvaderHealth(), Random.Range(2.5f, 4.0f)};
    
            var xPos = (side == 1 ? -4.4f : 0.7f) + row * 0.4f;
    
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

        private GameObject GetInvaderFromSide(int side)
        {
            GameObject invaderFromSide = null;
            
            foreach (var invader in GameObject.FindGameObjectsWithTag("Invader"))
                if (invader.GetComponent<InvaderMovement>().GetSide() == side)
                    invaderFromSide = invader;

            return invaderFromSide;
        }

        private float GetMoveInterval(Player player)
        {
            return 1.0f;
            //var round = (int) player.CustomProperties[PlayerStats.CurrentRound];
            //return new []{3.0f, 2.5f, 2.0f, 1.75f, 1.5f}[round - 1];
        }

        private void MoveInvadersInSide(int side, Vector2 direction)
        {
            foreach (var invader in GameObject.FindGameObjectsWithTag("Invader"))
            {
                var movement = invader.GetComponent<InvaderMovement>();

                if (movement.GetSide() != side) continue;
                
                movement.Move(direction);
            }
        }

        private void TurnAroundInvadersInSide(int side)
        {
            foreach (var invader in GameObject.FindGameObjectsWithTag("Invader"))
            {
                var movement = invader.GetComponent<InvaderMovement>();

                if (movement.GetSide() != side) continue;
                
                movement.ChangeDirection();
            }
        }
    }
}