﻿using System.Collections.Generic;
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

        private void Update()
        {
            var rect = OwnAreaRect();

            // Top line
            Debug.DrawLine(new Vector3(rect.x, rect.y), new Vector3(rect.x + rect.width, rect.y), Color.red);

            // Right line
            Debug.DrawLine(new Vector3(rect.x + rect.width, rect.y), new Vector3(rect.x + rect.width, rect.y + rect.height), Color.red);

            // Bottom line
            Debug.DrawLine(new Vector3(rect.x + rect.width, rect.y + rect.height), new Vector3(rect.x, rect.y + rect.height), Color.red);

            // Left line
            Debug.DrawLine(new Vector3(rect.x, rect.y + rect.height), new Vector3(rect.x, rect.y), Color.red);
        }

        #endregion

        public Vector3 OwnAreaPosition(float x, float y) =>
            PlayAreaPosition(_playerIndex, x, y);

        public Vector3 PlayAreaPosition(int key, float x, float y) =>
            _playAreas[key].transform.position + new Vector3(x, y, 0);

        public Rect OwnAreaRect()
        {
            var leftCorner = OwnAreaPosition(-2.0f, -2.3f);

            return new Rect(leftCorner.x, leftCorner.y, 4, 4.6f);
        }

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
