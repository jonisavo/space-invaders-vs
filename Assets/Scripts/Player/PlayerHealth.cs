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
        
        #region MonoBehaviour Callbacks

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (invincibility) return;
            
            if (!other.gameObject.CompareTag("EnemyBullet")) return;
            
            if (photonView.IsMine) GetHit();
            
            Destroy(other.gameObject);
        }
        
        #endregion

        private void GetHit()
        {
            PlayerStats.RemoveLife();
            
            photonView.RPC("SpawnExplosion", RpcTarget.All);
            
            // Reposition player using SpawnManager's functions
            transform.position = new Vector3(
                PhotonNetwork.LocalPlayer.ActorNumber == 1 ? -2.5f : 2.75f, -1.0f, 0
                );
        }
        
        [PunRPC]
        private void SpawnExplosion() =>
            Instantiate(explosion, transform.position, Quaternion.identity);
    }
}
