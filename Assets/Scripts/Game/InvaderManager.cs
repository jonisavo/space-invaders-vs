using System.Collections;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Random = UnityEngine.Random;

namespace SIVS
{
    [RequireComponent(typeof(SpawnManager))]
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

        [Tooltip("If debug mode is on, logs all invader movement.")]
        public bool debugLog = false;
        
        private int _totalInvaderKills = 0;

        private SpawnManager _spawnManager;

        private void Awake()
        {
            _spawnManager = GetComponent<SpawnManager>();
        }

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

        #endregion
        
        #region Coroutines
    
        private IEnumerator MoveInvaders()
        {
            var side = PhotonNetwork.LocalPlayer.ActorNumber;

            while (true)
            {
                yield return new WaitForSeconds(GetMoveInterval());

                var invader = GetOwnInvader();

                if (!invader) continue;

                var movement = invader.GetComponent<InvaderMovement>();

                if (debugLog)
                    Debug.Log($"Checking whether invaders on side {side} can move {(movement.GetMovementDirection().x > 0 ? "right" : "left")}");
                
                if (movement.CanMoveHorizontally())
                {
                    if (debugLog)
                        Debug.Log($"Moving invaders of side {side} horizontally");
                    MoveOwnInvaders(movement.GetMovementDirection());
                }
                else
                {
                    if (movement.CanMoveDown())
                    {
                        if (debugLog)
                            Debug.Log($"Moving invaders of side {side} vertically");
                        MoveOwnInvaders(Vector2.down);
                    }
                    TurnAroundOwnInvaders();
                }
            }
        }

        #endregion
        
        public void InitializeInvaders()
        {
            SpawnOwnInvaders();
            StartCoroutine(MoveInvaders());
        }

        private void SpawnOwnInvaders()
        {
            int rows, columns;
            
            if (debugMode)
            {
                rows = debugRows;
                columns = debugColumns;   
            }
            else
            {
                rows = 4 + PlayerStats.GetRound();
                columns = 4 + PlayerStats.GetRound() / 2;
            }

            for (var row = 0; row < rows; row++)
                for (var column = 0; column < columns; column++)
                    SpawnOneInvader(PhotonNetwork.LocalPlayer.ActorNumber, row, column);
        }
    
        private void SpawnOneInvader(int side, int row, int column)
        {
            object[] instantiationData = {side, GenerateInvaderHealth(), Random.Range(2.5f, 4.0f)};

            var position = _spawnManager.OwnAreaPosition(0.2f + row * 0.4f, 2.1f - column * 0.3f);

            PhotonNetwork.Instantiate("Invader",
                position, Quaternion.identity, 0, instantiationData);
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

        private GameObject GetOwnInvader()
        {
            GameObject invaderFromSide = null;

            foreach (var invader in GameObject.FindGameObjectsWithTag("Invader"))
                if (invader.GetPhotonView().IsMine)
                    invaderFromSide = invader;

            return invaderFromSide;
        }

        private float GetMoveInterval()
        {
            if (debugMode && debugMoveRate != 0) return debugMoveRate;
            
            return new []{3.0f, 2.5f, 2.0f, 1.75f, 1.5f}[PlayerStats.GetRound() - 1];
        }

        private void MoveOwnInvaders(Vector2 direction)
        {
            foreach (var invader in GameObject.FindGameObjectsWithTag("Invader"))
            {
                if (!invader.GetPhotonView().IsMine) continue;
                
                invader.GetComponent<InvaderMovement>().Move(direction);
            }
        }

        private void TurnAroundOwnInvaders()
        {
            foreach (var invader in GameObject.FindGameObjectsWithTag("Invader"))
            {
                if (!invader.GetPhotonView().IsMine) continue;

                invader.GetComponent<InvaderMovement>().ChangeDirection();
            }
        }
    }
}
