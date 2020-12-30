using System.Collections;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Random = UnityEngine.Random;

namespace SIVS
{
    public class InvaderManager : MonoBehaviourPunCallbacks
    {
        [Tooltip("Toggles debug options.")]
        public bool debugMode = false;

        [Tooltip("If debug mode is on, forces the row count.")]
        public int debugRows = 1;

        [Tooltip("If debug mode is on, forces the column count.")]
        public int debugColumns = 1;

        [Tooltip("If debug mode is on, forces the move rate. Set to 0 to ignore this setting.")]
        public float debugMoveRate = 1.0f;
        
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

                Debug.Log($"Checking whether invaders on side {side} can move {(movement.GetMovementDirection().x > 0 ? "right" : "left")}");
                if (movement.CanMoveHorizontally())
                {
                    Debug.Log($"Moving invaders of side {side} horizontally");
                    MoveInvadersInSide(side, movement.GetMovementDirection());
                }
                else
                {
                    if (movement.CanMoveDown())
                    {
                        Debug.Log($"Moving invaders of side {side} vertically");
                        MoveInvadersInSide(side, Vector2.down);
                    }
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
            int rows, columns;
            
            if (debugMode)
            {
                rows = debugRows;
                columns = debugColumns;   
            }
            else
            {
                rows = 4 + (int) player.CustomProperties[PlayerStats.CurrentRound];
                columns = 4 + (int) player.CustomProperties[PlayerStats.CurrentRound] / 2;
            }
            
            for (var row = 0; row < rows; row++)
                for (var column = 0; column < columns; column++)
                    SpawnOneInvader(player.ActorNumber, row, column);
        }
    
        private void SpawnOneInvader(int side, int row, int column)
        {
            object[] instantiationData = {side, GenerateInvaderHealth(), Random.Range(2.5f, 4.0f)};
    
            var xPos = (side == 1 ? -4.35f : 0.7f) + row * 0.4f;
    
            var yPos = 2.1f - column * 0.3f;
    
            PhotonNetwork.InstantiateRoomObject("Invader", new Vector3(xPos, yPos, 0),
                Quaternion.identity, 0, instantiationData);
        }
    
        private int GenerateInvaderHealth()
        {
            if (_totalInvaderKills < 15)
                return 1;
            
            if (_totalInvaderKills < 35)
                return Random.Range(1, 4);
            
            if (_totalInvaderKills < 60)
                return Random.Range(2, 6);
            
            return Random.Range(2, 10);
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
            if (debugMode && debugMoveRate != 0) return debugMoveRate;
            
            // take lag into account here for the other player (not the master client?)
            
            var round = (int) player.CustomProperties[PlayerStats.CurrentRound];
            return new []{3.0f, 2.5f, 2.0f, 1.75f, 1.5f}[round - 1];
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