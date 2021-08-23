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
        [Tooltip("GameObject containing a TextPopup component to show when a UFO is spawned.")]
        public GameObject textPopupObject;
        
        [Tooltip("Toggles debug options.")]
        public bool debugMode = false;

        [Tooltip("If debug mode is on, forces the row count. Set to 0 to ignore this setting.")]
        public int debugRows = 1;

        [Tooltip("If debug mode is on, forces the column count. Set to 0 to ignore this setting.")]
        public int debugColumns = 1;

        [Tooltip("If debug mode is on, forces the move rate. Set to 0 to ignore this setting.")]
        public float debugMoveRate = 1.0f;

        [Tooltip("If debug mode is on, logs all invader movement.")]
        public bool debugLog = false;

        private int _totalInvaderKills = 0;

        private SpawnManager _spawnManager;

        private static readonly float[] MovementIntervals = { 2.0f, 1.75f, 1.5f, 1.25f, 1.0f };

        #region Callbacks

        private void Awake()
        {
            _spawnManager = GetComponent<SpawnManager>();
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if (!Match.IsActive) return;

            if (!changedProps.ContainsKey(PlayerStats.InvaderKills)) return;

            var invaderKills = 0;

            foreach (var player in PhotonNetwork.CurrentRoom.Players.Values)
                if (player.CustomProperties.ContainsKey(PlayerStats.InvaderKills))
                    invaderKills += (int) player.CustomProperties[PlayerStats.InvaderKills];

            _totalInvaderKills = invaderKills;

            if (OwnInvaderCount() > 0) return;

            var nextRound = PlayerStats.GetOwnRound() + 1;

            PlayerStats.GoToNextRound(PhotonNetwork.LocalPlayer);

            if (nextRound < 6)
                SpawnOwnInvaders();
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

        private IEnumerator SpawnUFOs()
        {
            var ufosSpawned = 0;
            var side = PhotonNetwork.LocalPlayer.ActorNumber;

            while (ufosSpawned < 5)
            {
                yield return new WaitForSeconds(Random.Range(25.0f, 35.0f));

                if (!Match.IsActive) break;

                if (GameObject.FindGameObjectWithTag("UFO") != null)
                    continue;

                var position = _spawnManager.OwnAreaPosition(side == 1 ? -3.0f : 3.0f, 2.0f);

                object[] instantiationData = { side == 1 };

                PhotonNetwork.Instantiate("UFO",
                    position, Quaternion.identity, 0, instantiationData);
                
                photonView.RPC(nameof(UFOSpawned), RpcTarget.All);

                ufosSpawned++;
            }
        }

        #endregion

        [PunRPC]
        private void UFOSpawned()
        {
            var popupObject = Instantiate(textPopupObject, Vector3.zero, Quaternion.identity);
            
            popupObject.GetComponent<TextPopup>().Show("<animation=verticalpos>GET THAT UFO!</animation>");
        }

        public void InitializeInvaders()
        {
            SpawnOwnInvaders();
            StartCoroutine(MoveInvaders());
            StartCoroutine(SpawnUFOs());
        }

        private void SpawnOwnInvaders()
        {
            var rows = debugMode && debugRows > 0 ? debugRows : 3 + PlayerStats.GetOwnRound();

            var columns = debugMode && debugColumns > 0 ? debugColumns : 3 + PlayerStats.GetOwnRound() / 2;

            for (var row = 0; row < rows; row++)
                for (var column = 0; column < columns; column++)
                    SpawnOneInvader(PhotonNetwork.LocalPlayer.ActorNumber, row, column);
        }

        private void SpawnOneInvader(int side, int row, int column)
        {
            object[] instantiationData = {side, GenerateInvaderHealth(), Random.Range(3.0f, 4.75f)};

            var position = _spawnManager.OwnAreaPosition(-1.75f + row * 0.4f, 2.1f - column * 0.3f);

            PhotonNetwork.Instantiate("Invader",
                position, Quaternion.identity, 0, instantiationData);
        }

        private int GenerateInvaderHealth()
        {
            if (_totalInvaderKills < 15)
                return 1;

            if (_totalInvaderKills < 40)
                return Random.Range(1, 3);

            if (_totalInvaderKills < 75)
                return Random.Range(2, 4);

            return Random.Range(2, 5);
        }

        private GameObject GetOwnInvader()
        {
            GameObject invaderFromSide = null;

            foreach (var invader in GameObject.FindGameObjectsWithTag("Invader"))
            {
                if (invader.GetPhotonView().IsMine)
                {
                    invaderFromSide = invader;
                    break;
                }
            }

            return invaderFromSide;
        }

        private float GetMoveInterval()
        {
            if (debugMode && debugMoveRate != 0) return debugMoveRate;

            var round =
                Mathf.Clamp(PlayerStats.GetOwnRound() - 1, 0, MovementIntervals.Length - 1);

            return MovementIntervals[round];
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

        private int OwnInvaderCount()
        {
            var count = 0;

            foreach (var invader in GameObject.FindGameObjectsWithTag("Invader"))
                if (invader.GetPhotonView().IsMine) count++;

            return count;
        }
    }
}
