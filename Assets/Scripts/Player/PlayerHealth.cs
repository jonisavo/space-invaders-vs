using System.Collections;
using RedBlueGames.NotNull;
using UnityEngine;

namespace SIVS
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class PlayerHealth : MonoBehaviour
    {
        [Tooltip("GameObject to instantiate as the player's explosion.")]
        [NotNull]
        public GameObject explosion;

        [Tooltip("Explosion GameObject to instantiate when all lives are lost.")]
        [NotNull]
        public GameObject livesLostExplosionParticles;

        [Tooltip("Audio clip to play upon explosion.")]
        [NotNull]
        public AudioClip explosionSound;

        [Tooltip("Audio clip to play when all lives are lost and the big explosion happens.")]
        [NotNull]
        public AudioClip bigExplosionSound;

        [Tooltip("The player's sprite renderer.")]
        [NotNull]
        public SpriteRenderer spriteRenderer;

        [Tooltip("A debug option to make players invincible.")]
        public bool invincibility;

        public delegate void OnHitDelegate(SIVSPlayer player);

        public static event OnHitDelegate OnHit;

        private AudioSource _audioSource;

        private BoxCollider2D _boxCollider;

        private SpriteRenderer _spriteRenderer;

        private bool _invincibilityFrames;

        private SIVSPlayer _player;

        protected virtual void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _boxCollider = GetComponent<BoxCollider2D>();
        }

        private void Start() => _player = GetThisPlayer();

        private void OnEnable() => SIVSPlayer.OnLivesChange += HandleLivesChange;

        private void OnDisable() => SIVSPlayer.OnLivesChange -= HandleLivesChange;

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

        protected virtual void GetHit()
        {
            _audioSource.PlayOneShot(explosionSound);

            MakeInvincible();

            _player.BulletType = PlayerBulletType.Normal;

            OnHit?.Invoke(_player);

            _player.RemoveLife();
        }

        private void MakeInvincible()
        {
            StartCoroutine(InvincibilityFrames());
        }

        private IEnumerator InvincibilityFrames()
        {
            _invincibilityFrames = true;

            spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);

            yield return new WaitForSeconds(1.5f);

            spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

            _invincibilityFrames = false;
        }

        private void HandleLivesChange(SIVSPlayer player, int newLives)
        {
            if (newLives > 0 || player.Number != _player.Number)
                return;
            
            ExplodeAndDisable();
        }

        private void ExplodeAndDisable()
        {
            CameraShaker.ShakeAll(0.14f, 0.8f);

            var position = transform.position;
            
            Instantiate(explosion, position, Quaternion.identity);
            
            Instantiate(livesLostExplosionParticles, position, Quaternion.identity);

            _spriteRenderer.enabled = false;

            _boxCollider.enabled = false;

            FreezeFrame.Trigger(3f);
            
            _audioSource.PlayOneShot(bigExplosionSound);
        }
    }
}