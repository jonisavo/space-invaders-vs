using System.Collections;
using RedBlueGames.NotNull;
using UnityEngine;

namespace SIVS
{
    [RequireComponent(typeof(AudioSource))]
    public class UFOHealth : MonoBehaviour
    {
        private const int KillPoints = 1000;

        [Tooltip("GameObject to instantiate as the UFO's explosion.")]
        [NotNull]
        public GameObject explosion;
        
        [Tooltip("Particle effect GameObject to instantiate with the explosion.")]
        [NotNull]
        public GameObject explosionParticles;

        [Tooltip("Audio clip to play upon death.")]
        [NotNull]
        public AudioClip deathSound;

        [Tooltip("GameObject containing a TextPopup component to instantiate when the UFO is destroyed.")]
        [NotNull]
        public GameObject pointsObject;
        
        public delegate void OnKillDelegate(int killerPlayerNumber);

        public static event OnKillDelegate OnKill;

        private bool _hidden;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("PlayerBullet"))
                return;

            Destroy(other.gameObject);

            Die(other.gameObject.GetComponent<PlayerBullet>().Owner);
        }
        
        protected virtual void Die(SIVSPlayer killer)
        {
            if (_hidden) return;

            SoundPlayer.PlaySound(deathSound);

            killer.Score += KillPoints;
            
            OnKill?.Invoke(killer.Number);

            var currentPosition = transform.position;
            
            var pointsObj = Instantiate(pointsObject, currentPosition, Quaternion.identity);
            pointsObj.GetComponent<TextPopup>().Show($"<animation=slowsine>{KillPoints}</animation>");

            if (gameObject.TryGetComponent(out PowerupDrop drop))
                drop.GeneratePowerupDrop();

            ShakeCameraAndStartDestroy();

            Hide();

            Instantiate(explosion, currentPosition, Quaternion.identity);
            Instantiate(explosionParticles, currentPosition, Quaternion.identity);
        }

        protected virtual void ShakeCameraAndStartDestroy()
        {
            CameraShaker.ShakeAll(0.1f, 0.35f);
            StartCoroutine(DestroyDelay());
        }

        private IEnumerator DestroyDelay()
        {
            yield return new WaitForSeconds(0.5f);

            DestroyObject();
        }

        protected virtual void DestroyObject() => Destroy(gameObject);

        private void Hide()
        {
            _hidden = true;

            GetComponent<SpriteRenderer>().enabled = false;

            GetComponent<Collider2D>().enabled = false;
        }
    }
}