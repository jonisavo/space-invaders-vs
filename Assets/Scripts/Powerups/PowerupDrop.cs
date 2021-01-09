using Photon.Pun;
using UnityEngine;

namespace SIVS
{
    public class PowerupDrop : MonoBehaviourPun
    {
        public string[] powerupPrefabNames;

        public float[] dropChances;

        public float SpawnXOffset = 0.0f;

        public float SpawnYOffset = 0.0f;

        public void GeneratePowerupDrop()
        {
            if (!PhotonNetwork.InRoom || !photonView.IsMine)
                return;

            if (powerupPrefabNames.Length != dropChances.Length)
            {
                Debug.LogError("Powerup prefab names and drop chances don't match!");
                return;
            }

            var powerup = GetDrop();

            if (powerup == null) return;

            SpawnPowerup(powerup);
        }

        private string GetDrop()
        {
            for (var i = 0; i < powerupPrefabNames.Length; i++)
                if (Random.Range(0.0f, 100.0f) < dropChances[i] + 0.5f * Match.SumOfRounds)
                    return powerupPrefabNames[i];

            return null;
        }

        private void SpawnPowerup(string powerupName)
        {
            var position = transform.position + new Vector3(SpawnXOffset, SpawnYOffset);

            PhotonNetwork.Instantiate(powerupName, position, Quaternion.identity);
        }
    }
}