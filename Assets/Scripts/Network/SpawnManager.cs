using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace SIVS
{
    public class SpawnManager : MonoBehaviourPunCallbacks
    {
        private Dictionary<int, GameObject> _playAreas;

        private int _playerIndex = 1;
        
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

        private void SpawnShip()
        {
            object[] playerData = {PhotonNetwork.NickName};
            
            PhotonNetwork.Instantiate("PlayerShip",
                new Vector3(_playerIndex == 1 ? -2.5f : 2.65f, -1.5f, 0), 
                Quaternion.identity, 0, playerData);
        }

        private void SpawnCover()
        {
            PhotonNetwork.Instantiate("Cover", PlayAreaPosition(-0.9f, -1.16f), Quaternion.identity);

            PhotonNetwork.Instantiate("Cover", PlayAreaPosition(1.1f,-1.16f), Quaternion.identity);
        }

        private GameObject OwnPlayArea() => _playAreas[_playerIndex];

        private Vector3 PlayAreaPosition(float x, float y) =>
            OwnPlayArea().transform.position + new Vector3(x, y, 0);
    }
}
