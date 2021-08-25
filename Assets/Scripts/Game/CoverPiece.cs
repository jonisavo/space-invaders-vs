using Photon.Pun;
using UnityEngine;

namespace SIVS
{
    public class CoverPiece : MonoBehaviour
    {
        [Tooltip("Particle effect GameObject to instantiate when the cover piece is hit.")]
        public GameObject explosionParticles;

        [Tooltip("Sound effect to play with the explosion.")]
        public AudioClip explosionSound;
        
        private Cover _cover;

        private int _id;
        
        #region Unity Callbacks

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("PlayerBullet") && !other.gameObject.CompareTag("EnemyBullet"))
                return;
            
            Destroy(other.gameObject);

            Instantiate(explosionParticles, transform.position, Quaternion.identity);

            if (!_cover.photonView.IsMine) return;
            
            SoundPlayer.PlaySound(explosionSound, 0.65f);

            _cover.photonView.RPC("DestroyPiece", RpcTarget.AllBuffered, _id);
        }
        
        #endregion

        public void InitializeCoverPiece(int id, Cover coverComponent)
        {
            _id = id;
            _cover = coverComponent;
        }
    }
}
