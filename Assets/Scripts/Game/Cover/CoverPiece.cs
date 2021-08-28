using UnityEngine;

namespace SIVS
{
    public class CoverPiece : MonoBehaviour
    {
        [Tooltip("Particle effect GameObject to instantiate when the cover piece is hit.")]
        public GameObject explosionParticles;

        [Tooltip("Sound effect to play with the explosion.")]
        public AudioClip explosionSound;

        protected int _id;
        
        private Cover _cover;
        
        #region Unity Callbacks

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("PlayerBullet") && !other.gameObject.CompareTag("EnemyBullet"))
                return;
            
            Destroy(other.gameObject);

            Instantiate(explosionParticles, transform.position, Quaternion.identity);
            
            OnPieceHit();
        }
        
        #endregion

        protected virtual void OnPieceHit()
        {
            SoundPlayer.PlaySound(explosionSound, 0.65f);
            
            _cover.DestroyPiece(_id);
        }

        public void InitializeCoverPiece(int id, Cover coverComponent)
        {
            _id = id;
            _cover = coverComponent;
        }
    }
}
