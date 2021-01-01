using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace SIVS
{
    public class SpawnManager : MonoBehaviourPunCallbacks
    {
        private Dictionary<int, GameObject> _playAreas;

        private int _playerIndex = 1;
        
        #region MonoBehaviour Callbacks
        
        private void Awake()
        {
            _playAreas = new Dictionary<int, GameObject>();
            
            foreach (var entry in PhotonNetwork.CurrentRoom.Players)
            {
                if (entry.Value.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
                {
                    _playerIndex = entry.Key;
                }
                
                _playAreas[entry.Key] = GameObject.Find("Play Area " + entry.Key);
            }
        }
        
        private void Start()
        {
            SpawnShip();
            SpawnCover();
        }
        
        #endregion

        public Vector3 OwnAreaPosition(float x, float y) => 
            PlayAreaPosition(_playerIndex, x, y);

        public Vector3 PlayAreaPosition(int key, float x, float y) =>
            _playAreas[key].transform.position + new Vector3(x, y, 0);

        private void SpawnShip()
        {
            object[] playerData = {PhotonNetwork.NickName};

            PhotonNetwork.Instantiate("PlayerShip",
                OwnAreaPosition(0.0f, -1.5f),
                Quaternion.identity, 0, playerData);
        }

        private void SpawnCover()
        {
            PhotonNetwork.Instantiate("Cover", OwnAreaPosition(-1.7f, -1.12f), Quaternion.identity);

            PhotonNetwork.Instantiate("Cover", OwnAreaPosition(0.5f,-1.12f), Quaternion.identity);
        }
    }
}
