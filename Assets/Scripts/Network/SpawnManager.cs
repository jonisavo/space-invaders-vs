using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace SIVS
{
    public class SpawnManager : MonoBehaviourPunCallbacks
    {
        private Dictionary<int, GameObject> _playAreas;

        private int _playerIndex = 1;

        #region Unity Callbacks

        private void Awake()
        {
            _playAreas = new Dictionary<int, GameObject>();
        }

        private void Start()
        {
            InitPlayAreas();
            SpawnShip();
            SpawnCover();
        }

        private void InitPlayAreas()
        {
            foreach (var entry in PhotonNetwork.CurrentRoom.Players)
            {
                if (entry.Value.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
                {
                    _playerIndex = entry.Key;
                }
                
                var playAreaObject = GameObject.Find("Play Area " + entry.Key);
                
                playAreaObject.GetComponent<PlayArea>().Initialize(entry.Value, entry.Key);
                
                playAreaObject.GetComponent<PlayerInfoUI>()
                    .Initialize(entry.Value);

                _playAreas[entry.Key] = playAreaObject;
            }
        }

        public override void OnEnable()
        {
            base.OnEnable();
            PlayerHealth.OnSelfHit += RespawnSelf;
        }
        
        public override void OnDisable()
        {
            base.OnDisable();
            PlayerHealth.OnSelfHit -= RespawnSelf;
        }

        #endregion

        private void RespawnSelf(GameObject playerObject) =>
            playerObject.transform.position = OwnSpawnPoint();

        public Vector3 OwnAreaPosition(float x, float y) =>
            PlayAreaPosition(_playerIndex, x, y);

        public Vector3 PlayAreaPosition(int key, float x, float y) =>
            _playAreas[key].transform.position + new Vector3(x, y, 0);

        public Rect PlayAreaRect(int key)
        {
            var leftCorner = PlayAreaPosition(key, -2.0f, -2.3f);

            return new Rect(leftCorner.x, leftCorner.y, 4, 4.6f);
        }

        public Rect OwnAreaRect() => PlayAreaRect(_playerIndex);

        public Vector3 OwnSpawnPoint() => OwnAreaPosition(0.0f, -1.6f);

        private void SpawnShip()
        {
            object[] playerData = {PhotonNetwork.NickName};

            PhotonNetwork.Instantiate("PlayerShip",
                OwnSpawnPoint(), Quaternion.identity, 0, playerData);
        }

        private void SpawnCover()
        {
            PhotonNetwork.Instantiate("Cover", OwnAreaPosition(-1.7f, -1.12f), Quaternion.identity);

            PhotonNetwork.Instantiate("Cover", OwnAreaPosition(0.5f,-1.12f), Quaternion.identity);
        }
    }
}
