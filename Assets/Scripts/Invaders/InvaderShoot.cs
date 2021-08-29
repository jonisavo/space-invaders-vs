using System.Collections;
using UnityEngine;

namespace SIVS
{
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(AudioSource))]
    public class InvaderShoot : MonoBehaviour
    {
        [Tooltip("The bullet to shoot.")]
        public GameObject bullet;

        [Tooltip("Audio clip to play when shooting.")]
        public AudioClip shootSound;

        private Vector3 _distanceToShootPoint;

        private float _shootInterval = 1.5f;

        private AudioSource _audioSource;

        protected virtual void Awake()
        {
            var bounds = GetComponent<BoxCollider2D>().bounds;
            _distanceToShootPoint = bounds.min - transform.position;
            _distanceToShootPoint.x += bounds.size.x / 2;
            _distanceToShootPoint.y -= 0.05f;

            _audioSource = GetComponent<AudioSource>();

            _shootInterval = Random.Range(3.0f, 4.75f);

            StartShooting();
        }

        protected virtual void StartShooting() =>
            StartCoroutine(ShootCoroutine());

        private IEnumerator ShootCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(_shootInterval);
                
                var hit = Physics2D.Raycast(GetBulletSpawnPoint(), Vector2.down,
                    Mathf.Infinity, LayerMask.GetMask("Invaders"));

                if (hit.collider) continue;
            
                Shoot();
            }
        }

        public virtual void StopShooting()
        {
            StopAllCoroutines();
        }
        
        protected virtual void Shoot()
        {
            PlayShootSound();
            
            Instantiate(bullet, GetBulletSpawnPoint(), Quaternion.identity);    
        }

        protected virtual void PlayShootSound()
        {
            _audioSource.PlayOneShot(shootSound, 0.6f);
        }
        
        private Vector2 GetBulletSpawnPoint()
        {
            return transform.position + _distanceToShootPoint;
        }
    }
}
