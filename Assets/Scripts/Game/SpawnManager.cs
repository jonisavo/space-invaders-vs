using System.Collections.Generic;
using UnityEngine;

namespace SIVS
{
    public class SpawnManager : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Player ship to instantiate in local multiplayer.")]
        private GameObject localPlayerShipObject;

        [SerializeField]
        [Tooltip("Cover object to instantiate in local multiplayer.")]
        private GameObject localCoverObject;
        
        private Dictionary<int, GameObject> _playAreas;

        #region Unity Callbacks

        private void Awake()
        {
            _playAreas = new Dictionary<int, GameObject>();
        }

        private void Start()
        {
            InitPlayAreas();
            SpawnShips();
            SpawnCover();
        }

        private void InitPlayAreas()
        {
            foreach (var entry in GameManager.Players)
            {
                var playAreaObject = GameObject.Find("Play Area " + entry.Key);
                
                playAreaObject.GetComponent<PlayArea>().Initialize(entry.Value);
                
                playAreaObject.GetComponent<PlayerInfoUI>()
                    .Initialize(entry.Value);

                _playAreas[entry.Key] = playAreaObject;
            }
        }

        protected void OnEnable() => PlayerHealth.OnHit += RespawnPlayer;

        protected void OnDisable() => PlayerHealth.OnHit -= RespawnPlayer;

        #endregion

        protected virtual void RespawnPlayer(SIVSPlayer player, GameObject playerObject) =>
            playerObject.transform.position = PlayerSpawnPoint(player.Number);

        public Vector3 PlayAreaPosition(int playerNumber, float x, float y) =>
            _playAreas[playerNumber].transform.position + new Vector3(x, y, 0);

        public Rect PlayAreaRect(int key)
        {
            var leftCorner = PlayAreaPosition(key, -2.0f, -2.3f);

            return new Rect(leftCorner.x, leftCorner.y, 4f, 4.6f);
        }

        public Vector3 PlayerSpawnPoint(int playerNumber) =>
            PlayAreaPosition(playerNumber, 0.0f, -1.6f);

        protected virtual void SpawnShips()
        {
            var p1 = Instantiate(localPlayerShipObject, PlayerSpawnPoint(1), Quaternion.identity);
            p1.GetComponent<Ownership>().Owner = GameManager.Players[1];

            var p2 = Instantiate(localPlayerShipObject, PlayerSpawnPoint(2), Quaternion.identity);
            p2.GetComponent<Ownership>().Owner = GameManager.Players[2];
            
            GameManager.Players[1].InitializeStats();
            GameManager.Players[2].InitializeStats();
        }

        protected virtual void SpawnCover()
        {
            SpawnOneLocalCover(1, -1.7f, -1.12f);

            SpawnOneLocalCover(1, 0.5f, -1.12f);

            SpawnOneLocalCover(2, -1.7f, -1.12f);

            SpawnOneLocalCover(2, 0.5f, -1.12f);
        }

        private void SpawnOneLocalCover(int playerNumber, float x, float y)
        {
            var cover =
                Instantiate(localCoverObject, PlayAreaPosition(playerNumber, x, y), Quaternion.identity);

            cover.GetComponent<Ownership>().Owner = GameManager.Players[playerNumber];
        }
    }
}
