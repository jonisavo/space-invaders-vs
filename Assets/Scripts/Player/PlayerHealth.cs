using System.Collections;
using RedBlueGames.NotNull;
using UnityEngine;

namespace SIVS
{
    [RequireComponent(typeof(AudioSource))]
    public class PlayerHealth : MonoBehaviour
    {
        [Tooltip("GameObject to instantiate as the player's explosion.")]
        [NotNull]
        public GameObject explosion;

        [Tooltip("Audio clip to play upon explosion.")]
        [NotNull]
        public AudioClip explosionSound;

        [Tooltip("The player's sprite renderer.")]
        [NotNull]
        public SpriteRenderer spriteRenderer;

        [Tooltip("A debug option to make players invincible.")]
        public bool invincibility;

        public delegate void OnHitDelegate(SIVSPlayer player, GameObject playerGameObject);

        public static event OnHitDelegate OnHit;

        private AudioSource _audioSource;

        private bool _invincibilityFrames;

        private SIVSPlayer _player;

        protected virtual void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _player = GetThisPlayer();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (invincibility) return;

            if (!other.gameObject.CompareTag("EnemyBullet"))
                return;

            Destroy(other.gameObject);

            if (ShouldRegisterHit())
                GetHit();
        }

        protected virtual SIVSPlayer GetThisPlayer() => GetComponent<Ownership>().Owner;

        protected virtual bool ShouldRegisterHit() => !_invincibilityFrames;
        
        protected virtual void MakeInvincible()
        {
            StartCoroutine(InvincibilityFrames());
        }

        private IEnumerator InvincibilityFrames()
        {
            _invincibilityFrames = true;

            spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);

            yield return new WaitForSeconds(2.0f);

            spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

            _invincibilityFrames = false;
        }

        private void GetHit()
        {
            _audioSource.PlayOneShot(explosionSound);

            SpawnExplosion();

            MakeInvincible();
            
            _player.BulletType = PlayerBulletType.Normal;

            OnHit?.Invoke(_player, gameObject);
            
            _player.RemoveLife();
        }
        
        protected virtual void SpawnExplosion() =>
            Instantiate(explosion, transform.position, Quaternion.identity);
    }
}