using UnityEngine;
using Photon.Pun;

namespace SIVS
{
    [RequireComponent(typeof(AudioSource))]
    public class PlayerHealth : MonoBehaviourPunCallbacks
    {
        [Tooltip("GameObject to instantiate as the player's explosion.")]
        public GameObject explosion;

        [Tooltip("Audio clip to play upon explosion.")]
        public AudioClip explosionSound;
        
        [Tooltip("A debug option to make players invincible.")]
        public bool invincibility = false;

        private SpawnManager _spawnManager;

        private AudioSource _audioSource;
        
        #region Unity Callbacks

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
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
            _audioSource.PlayOneShot(explosionSound);
            
            photonView.RPC(nameof(SpawnExplosion), RpcTarget.All);

            transform.position = _spawnManager.OwnAreaPosition(0.0f, -1.5f);
            
            PlayerStats.RemoveLife();
        }
        
        [PunRPC]
        private void SpawnExplosion() =>
            Instantiate(explosion, transform.position, Quaternion.identity);
    }
}
