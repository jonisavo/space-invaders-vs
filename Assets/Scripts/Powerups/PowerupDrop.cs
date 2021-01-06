﻿using Photon.Pun;
using UnityEngine;

namespace SIVS
{
    public class PowerupDrop : MonoBehaviourPun
    {
        public float LifePowerupDropChance = 0.0f;

        public float SpawnXOffset = 0.0f;

        public float SpawnYOffset = 0.0f;

        private void OnDestroy()
        {
            if (!photonView.IsMine) return;
            
            var powerup = GetDrop();

            if (powerup == null) return;
            
            SpawnPowerup(powerup);
        }

        private string GetDrop()
        {
            if (Random.Range(0.0f, 100.0f) < LifePowerupDropChance)
                return "Life Powerup";

            return null;
        }

        private void SpawnPowerup(string powerupName)
        {
            var position = transform.position + new Vector3(SpawnXOffset, SpawnYOffset);

            PhotonNetwork.Instantiate(powerupName, position, Quaternion.identity);
        }
    }
}