using Photon.Pun;
using UnityEngine;

namespace SIVS
{
    public class PowerupDrop : MonoBehaviourPun
    {
        public float LifePowerupDropChance = 0.0f;

        public float CoverPowerupDropChance = 0.0f;

        public float SpawnXOffset = 0.0f;

        public float SpawnYOffset = 0.0f;

        private void OnDestroy()
        {
            if (!PhotonNetwork.InRoom || !photonView.IsMine) return;
            
            var powerup = GetDrop();

            if (powerup == null) return;
            
            SpawnPowerup(powerup);
        }

        // TODO: Figure out a better system for generating powerup drops
        private string GetDrop()
        {
            if (Random.Range(0.0f, 100.0f) < LifePowerupDropChance + 0.5f * Match.SumOfRounds)
                return "Life Powerup";

            if (Random.Range(0.0f, 100.0f) < CoverPowerupDropChance + 0.5f * Match.SumOfRounds)
                return "Cover Powerup";

            return null;
        }

        private void SpawnPowerup(string powerupName)
        {
            var position = transform.position + new Vector3(SpawnXOffset, SpawnYOffset);

            PhotonNetwork.Instantiate(powerupName, position, Quaternion.identity);
        }
    }
}