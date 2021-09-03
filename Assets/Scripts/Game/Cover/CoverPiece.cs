﻿using UnityEngine;

namespace SIVS
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class CoverPiece : MonoBehaviour
    {
        [Tooltip("Particle effect GameObject to instantiate when the cover piece is hit.")]
        public GameObject explosionParticles;

        [Tooltip("Sound effect to play with the explosion.")]
        public AudioClip explosionSound;

        protected int _id;
        
        protected Cover _cover;

        protected Rigidbody2D _rb;

        private void Awake() => _rb = GetComponent<Rigidbody2D>();

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("PlayerBullet") && !other.gameObject.CompareTag("EnemyBullet"))
                return;
            
            Destroy(other.gameObject);

            if (explosionParticles)
                Instantiate(explosionParticles, transform.position, Quaternion.identity);
            
            OnPieceHit();
        }

        protected virtual void OnPieceHit()
        {
            if (explosionSound)
                SoundPlayer.PlaySound(explosionSound, 0.65f);
            
            _cover.DestroyPiece(_id);
        }

        public void InitializeCoverPiece(int id, Cover coverComponent)
        {
            _id = id;
            _cover = coverComponent;
        }

        public void MakeRigidbodyDynamic()
        {
            _rb.bodyType = RigidbodyType2D.Dynamic;
            
            _rb.AddForce(Random.insideUnitCircle * Random.Range(4.5f, 6.5f));
        }
    }
}
