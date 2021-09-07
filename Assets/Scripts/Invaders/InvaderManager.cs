using System.Collections;
using RedBlueGames.NotNull;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SIVS
{
    [RequireComponent(typeof(SpawnManager))]
    public class InvaderManager : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Invader object to spawn.")]
        private GameObject invaderObject;

        [SerializeField]
        [Tooltip("UFO object to spawn.")]
        private GameObject ufoObject;
        
        [Tooltip("GameObject containing a TextPopup component to show when a UFO is spawned.")]
        [NotNull]
        public GameObject textPopupObject;

        [Tooltip("Audio clip that plays when the very last invader is killed.")]
        [NotNull]
        public AudioClip lastInvaderKillSound;
        
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
        
        protected SpawnManager _spawnManager;

        private int _totalInvaderKills = 0;

        private static readonly float[] MovementIntervals = { 2.0f, 1.75f, 1.5f, 1.25f, 1.0f };

        #region Callbacks

        protected virtual void Awake() => _spawnManager = GetComponent<SpawnManager>();

        private void OnEnable()
        {
            SIVSPlayer.OnInvaderKillsChange += HandleInvaderKillsChange;
            InvaderHealth.OnDeath += HandleInvaderDeath;
        }

        private void OnDisable()
        {
            SIVSPlayer.OnInvaderKillsChange -= HandleInvaderKillsChange;
            InvaderHealth.OnDeath -= HandleInvaderDeath;
        }

        private void HandleInvaderDeath(int killerPlayerNumber, GameObject diedInvader)
        {
            var player = GameManager.Players[killerPlayerNumber];

            if (player.CurrentRound == Match.FinalRound && GetInvaderCountOfPlayer(player) <= 1)
                PlayLastInvaderKillEffects(diedInvader.transform.position);
        }

        private void PlayLastInvaderKillEffects(Vector3 position)
        {
            FreezeFrame.Trigger(position, 1f);
            SoundPlayer.PlaySound(lastInvaderKillSound);
        }

        private void HandleInvaderKillsChange(SIVSPlayer player, int newKills)
        {
            if (!Match.IsActive) return;

            var invaderKills = 0;

            foreach (var roomPlayer in GameManager.Players.Values)
                invaderKills += roomPlayer.InvaderKills;

            _totalInvaderKills = invaderKills;
            
            if (!AreAllInvadersDefeated(player)) return;

            var nextRound = player.CurrentRound + 1;

            player.GoToNextRound();

            if (nextRound <= Match.FinalRound)
                SpawnInvadersForPlayer(player);
        }

        protected virtual bool AreAllInvadersDefeated(SIVSPlayer player) =>
            GetInvaderCountOfPlayer(player) <= 1;

        #endregion

        #region Coroutines

        private IEnumerator MoveInvaders(SIVSPlayer player)
        {
            while (true)
            {
                yield return new WaitForSeconds(GetMoveIntervalForPlayer(player));

                var invader = GetInvaderForPlayer(player);

                if (!invader) continue;

                var movement = invader.GetComponent<InvaderMovement>();

                if (movement.CanMoveHorizontally())
                {
                    if (debugLog)
                        Debug.Log($"Moving invaders of side {player.Number} horizontally");
                    MoveInvadersOfPlayer(player, movement.GetMovementDirection());
                }
                else
                {
                    if (movement.CanMoveDown())
                    {
                        if (debugLog)
                            Debug.Log($"Moving invaders of side {player.Number} vertically");
                        MoveInvadersOfPlayer(player, Vector2.down);
                    }
                    TurnAroundInvadersOfPlayer(player);
                }
            }
        }

        private IEnumerator SpawnUFOs(SIVSPlayer player)
        {
            var ufosSpawned = 0;

            while (ufosSpawned < 5)
            {
                yield return new WaitForSeconds(Random.Range(25.0f, 35.0f));

                if (!Match.IsActive) break;

                if (GameObject.FindGameObjectWithTag("UFO") != null)
                    continue;

                SpawnUFOForPlayer(player);

                ufosSpawned++;
            }
        }

        #endregion

        protected virtual void SpawnUFOForPlayer(SIVSPlayer player)
        {
            var spawnXCoord = player.Number == 1 ? -3.0f : 3.0f;
            var position = _spawnManager.PlayAreaPosition(player.Number, spawnXCoord, 2.0f);

            var ufo = Instantiate(ufoObject, position, Quaternion.identity);

            ufo.GetComponent<Ownership>().Owner = player;
                
            ShowUFOSpawnedPopup();
        }
        
        protected void ShowUFOSpawnedPopup()
        {
            var popupObject = Instantiate(textPopupObject, Vector3.zero, Quaternion.identity);
            
            popupObject.GetComponent<TextPopup>().Show("<animation=verticalpos>GET THAT UFO!</animation>");
        }

        public virtual void InitializeAllInvaders()
        {
            InitializeInvadersForPlayer(GameManager.Players[1]);
            InitializeInvadersForPlayer(GameManager.Players[2]);
        }

        protected void InitializeInvadersForPlayer(SIVSPlayer player)
        {
            SpawnInvadersForPlayer(player);
            StartCoroutine(MoveInvaders(player));
            StartCoroutine(SpawnUFOs(player));
        }

        protected virtual void SpawnInvadersForPlayer(SIVSPlayer player)
        {
            var round = player.CurrentRound;

            var rows = debugMode && debugRows > 0 ? debugRows : 3 + round;

            var columns = debugMode && debugColumns > 0 ? debugColumns : 3 + round / 2;

            for (var row = 0; row < rows; row++)
                for (var column = 0; column < columns; column++)
                    SpawnOneInvaderForPlayer(player, row, column);
        }

        protected virtual void SpawnOneInvaderForPlayer(SIVSPlayer player, int row, int column)
        {
            var position = _spawnManager.PlayAreaPosition(
                player.Number, -1.75f + row * 0.4f, 2.1f - column * 0.3f
            );

            var invader = Instantiate(invaderObject, position, Quaternion.identity);

            invader.GetComponent<Ownership>().Owner = player;
            
            invader.GetComponent<InvaderHealth>().InitializeHealth(GenerateInvaderHealth());
        }

        protected int GenerateInvaderHealth()
        {
            if (_totalInvaderKills < 15)
                return 1;

            if (_totalInvaderKills < 40)
                return Random.Range(1, 3);

            if (_totalInvaderKills < 75)
                return Random.Range(2, 4);

            return Random.Range(2, 5);
        }

        private GameObject GetInvaderForPlayer(SIVSPlayer player)
        {
            GameObject invaderForPlayer = null;

            foreach (var invader in GameObject.FindGameObjectsWithTag("Invader"))
            {
                if (InvaderOwnedByPlayer(invader, player))
                {
                    invaderForPlayer = invader;
                    break;
                }
            }

            return invaderForPlayer;
        }

        private float GetMoveIntervalForPlayer(SIVSPlayer player)
        {
            if (debugMode && debugMoveRate != 0)
                return debugMoveRate;

            var round = Mathf.Clamp(player.CurrentRound - 1, 0, MovementIntervals.Length - 1);

            return MovementIntervals[round];
        }

        private void MoveInvadersOfPlayer(SIVSPlayer player, Vector2 direction)
        {
            foreach (var invader in GameObject.FindGameObjectsWithTag("Invader"))
            {
                if (!InvaderOwnedByPlayer(invader, player))
                    continue;

                invader.GetComponent<InvaderMovement>().Move(direction);
            }
        }

        private void TurnAroundInvadersOfPlayer(SIVSPlayer player)
        {
            foreach (var invader in GameObject.FindGameObjectsWithTag("Invader"))
            {
                if (!InvaderOwnedByPlayer(invader, player))
                    continue;

                invader.GetComponent<InvaderMovement>().ChangeDirection();
            }
        }

        protected int GetInvaderCountOfPlayer(SIVSPlayer player)
        {
            var count = 0;

            foreach (var invader in GameObject.FindGameObjectsWithTag("Invader"))
                if (InvaderOwnedByPlayer(invader, player))
                    count++;

            return count;
        }

        protected virtual bool InvaderOwnedByPlayer(GameObject invader, SIVSPlayer player)
        {
            return invader.GetComponent<Ownership>().Owner.Number == player.Number;
        }
    }
}
