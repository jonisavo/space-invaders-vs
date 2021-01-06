using UnityEngine;
using Photon.Pun;

namespace SIVS
{
    public class PlayerHealth : MonoBehaviourPunCallbacks
    {
        [Tooltip("GameObject to instantiate as the player's explosion.")]
        public GameObject explosion;
        
        [Tooltip("A debug option to make players invincible.")]
        public bool invincibility = false;

        private SpawnManager _spawnManager;
        
        #region Unity Callbacks

        private void Awake()
        {
            _spawnManager = GameObject.Find("Game Manager").GetComponent<SpawnManager>();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (invincibility) return;
            
            if (!other.gameObject.CompareTag("EnemyBullet"))
                return;
            
            if (photonView.IsMine) GetHit();
            
            Destroy(other.gameObject);
        }
        
        #endregion

        private void GetHit()
        {
            photonView.RPC(nameof(SpawnExplosion), RpcTarget.All);

            transform.position = _spawnManager.OwnAreaPosition(0.0f, -1.5f);
            
            PlayerStats.RemoveLife();
        }
        
        [PunRPC]
        private void SpawnExplosion() =>
            Instantiate(explosion, transform.position, Quaternion.identity);
    }
}
