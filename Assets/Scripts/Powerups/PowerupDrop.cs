using UnityEngine;

namespace SIVS
{
    public class PowerupDrop : MonoBehaviour
    {
        public GameObject[] localPowerupPrefabs;

        public float[] dropChances;

        public float spawnXOffset;

        public float spawnYOffset;

        public virtual void GeneratePowerupDrop()
        {
            if (!ValidatePowerups())
                return;

            SpawnPowerup();
        }

        protected virtual bool ValidatePowerups()
        {
            if (localPowerupPrefabs.Length == dropChances.Length)
                return true;
            
            Debug.LogError("Powerup prefab objects and drop chances don't match!");
            
            return false;
        }

        protected virtual void SpawnPowerup()
        {
            var powerup = GetDrop();

            if (powerup == null)
                return;
            
            var position = transform.position + new Vector3(spawnXOffset, spawnYOffset);

            Instantiate(powerup, position, Quaternion.identity);
        }
        
        private GameObject GetDrop()
        {
            for (var i = 0; i < localPowerupPrefabs.Length; i++)
                if (Random.Range(0.0f, 100.0f) < dropChances[i] + 0.5f * Match.SumOfRounds)
                    return localPowerupPrefabs[i];

            return null;
        }
    }
}