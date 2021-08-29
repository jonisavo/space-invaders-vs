using Photon.Pun;
using UnityEngine;

namespace SIVS
{
    [RequireComponent(typeof(PhotonView))]
    public class PowerupDropOnline : PowerupDrop
    {
        public string[] powerupPrefabNames;
        
        private PhotonView _photonView;

        private void Awake() => _photonView = GetComponent<PhotonView>();
        
        protected override bool ValidatePowerups()
        {
            if (powerupPrefabNames.Length == dropChances.Length)
                return true;
            
            Debug.LogError("Powerup prefab names and drop chances don't match!");
            
            return false;
        }
        
        public override void GeneratePowerupDrop()
        {
            if (!PhotonNetwork.InRoom || !_photonView.IsMine)
                return;
            
            base.GeneratePowerupDrop();
        }

        protected override void SpawnPowerup()
        {
            var powerupName = GetDrop();

            if (string.IsNullOrEmpty(powerupName))
                return;
            
            var position = transform.position + new Vector3(spawnXOffset, spawnYOffset);

            PhotonNetwork.Instantiate(powerupName, position, Quaternion.identity);
        }
        
        private string GetDrop()
        {
            for (var i = 0; i < powerupPrefabNames.Length; i++)
                if (Random.Range(0.0f, 100.0f) < dropChances[i] + 0.5f * Match.SumOfRounds)
                    return powerupPrefabNames[i];

            return null;
        }
    }
}