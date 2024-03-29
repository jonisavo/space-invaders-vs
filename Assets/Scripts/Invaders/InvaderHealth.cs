﻿using System.Collections;
using RedBlueGames.NotNull;
using UnityEngine;

namespace SIVS
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class InvaderHealth : MonoBehaviour
    {
        [Tooltip("Toggles whether to automatically tint the invader sprite red.")]
        public bool tintSprite = true;

        [Tooltip("GameObject to instantiate as the invader's explosion.")]
        [NotNull]
        public GameObject explosion;

        [Tooltip("Particle effect GameObject to instantiate with the explosion.")]
        [NotNull]
        public GameObject explosionParticles;

        [Tooltip("Particle effect GameObject to instantiate with hits that don't result in death.")]
        [NotNull]
        public GameObject hitParticles;

        [Tooltip("Audio clip to play when losing health (without dying).")]
        [NotNull]
        public AudioClip hurtSound;

        [Tooltip("Audio clip to play upon death.")]
        [NotNull]
        public AudioClip deathSound;

        [Tooltip("Popup to instantiate for displaying points.")]
        [NotNull]
        public GameObject pointsPopup;

        private int _health;

        private int _initialHealth;

        private SpriteRenderer _spriteRenderer;
        
        private int _playerNumber;

        public delegate void DeathDelegate(int killerPlayerNumber, GameObject invaderObject);

        public static event DeathDelegate OnDeath;

        protected virtual void Awake() => _spriteRenderer = GetComponent<SpriteRenderer>();
        
        private void Start() => _playerNumber = GetPlayerNumber();

        protected virtual int GetPlayerNumber() => GetComponent<Ownership>().Owner.Number;

        private void OnEnable() => SIVSPlayer.OnLivesChange += HandlePlayerLivesChange;

        private void OnDisable() => SIVSPlayer.OnLivesChange -= HandlePlayerLivesChange;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("PlayerBullet"))
                return;

            Destroy(other.gameObject);
            
            OnBulletHit(other.gameObject.GetComponent<PlayerBullet>().Owner);
        }

        public void InitializeHealth(int health)
        {
            _health = health;
            _initialHealth = health;
            
            TintSprite();
        }
        
        protected virtual void OnBulletHit(SIVSPlayer player)
        {
            LoseHealth(player.Number);
        }
        
        protected void LoseHealth(int playerWhoHitNumber)
        {
            _health--;

            if (IsDead())
            {
                Die(GameManager.Players[playerWhoHitNumber]);
            }
            else
            {
                PlayHurtSoundAndShake();
                
                SpawnParticleObject(hitParticles);

                TintSprite();
            }
        }

        protected virtual void PlayHurtSoundAndShake()
        {
            SoundPlayer.PlaySound(hurtSound);
            CameraShaker.ShakeAll(0.04f, 0.1f);
        }

        private void Die(SIVSPlayer killer)
        {
            if (gameObject.TryGetComponent(out PowerupDrop drop))
                drop.GeneratePowerupDrop();

            var pointsToGive = 50 * _initialHealth;

            var pointsObj = Instantiate(pointsPopup, GetCenterPoint(), Quaternion.identity);
            pointsObj.GetComponent<TextPopup>().Show(pointsToGive.ToString());
            
            PlayDeathSoundAndShake();
            
            OnDeath?.Invoke(killer.Number, gameObject);
            
            GivePointsToKiller(killer, pointsToGive);

            ExplodeAndDestroy(killer);
        }

        public void ExplodeAndDestroy(SIVSPlayer killer)
        {
            SpawnExplosion();
            
            StartCoroutine(DestroyDelay(killer));
            
            Hide();
        }

        public void ExplodeAndDestroy() => ExplodeAndDestroy(null);

        protected virtual void GivePointsToKiller(SIVSPlayer killer, int points) =>
            killer.AddScore(points);

        protected virtual void PlayDeathSoundAndShake()
        {
            SoundPlayer.PlaySound(deathSound);
            CameraShaker.ShakeAll(0.08f, 0.3f);
        }

        private void Hide()
        {
            _spriteRenderer.enabled = false;

            GetComponent<Collider2D>().enabled = false;

            if (TryGetComponent(out InvaderShoot shootManager))
                shootManager.StopShooting();
        }

        private IEnumerator DestroyDelay(SIVSPlayer killer)
        {
            yield return new WaitForSeconds(0.5f);

            DestroyObject(killer);
        }

        protected virtual void DestroyObject(SIVSPlayer killer)
        {
            if (killer != null)
                killer.InvaderKills += 1;
            
            Destroy(gameObject);
        }

        private void TintSprite()
        {
            if (!tintSprite) return;

            var hue = 1.0f - (_health - 1) * 0.25f;
            _spriteRenderer.color = new Color(1.0f, hue, hue);
        }

        private void SpawnExplosion()
        {
            SpawnParticleObject(explosion);
            SpawnParticleObject(explosionParticles);
        }
        
        private void SpawnParticleObject(GameObject particleObject) =>
            Instantiate(particleObject, GetCenterPoint(), Quaternion.identity);
        
        private Vector2 GetCenterPoint()
        {
            var size = _spriteRenderer.size;
            
            return (Vector2) transform.position + new Vector2(size.x / 2, -size.y / 2);
        }

        protected bool IsDead() => _health <= 0;

        private void HandlePlayerLivesChange(SIVSPlayer player, int newLives)
        {
            if (player.Number != _playerNumber)
                return;
            
            if (newLives == 0)
                ExplodeAndDestroy();
        }
    }
}