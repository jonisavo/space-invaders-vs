using System.Collections;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace SIVS
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class InvaderHealth : MonoBehaviourPunCallbacks
    {
        [Tooltip("Toggles whether to automatically tint the invader sprite red.")]
        public bool tintSprite = true;

        [Tooltip("GameObject to instantiate as the invader's explosion.")]
        public GameObject explosion;

        [Tooltip("Audio clip to play when losing health (without dying).")]
        public AudioClip hurtSound;

        [Tooltip("Audio clip to play upon death.")]
        public AudioClip deathSound;

        private int _health;

        private int _initialHealth;

        private int _killerActorNumber;

        private bool _hidden;

        private SpriteRenderer _spriteRenderer;

        #region Unity Callbacks

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();

            if (photonView.InstantiationData != null)
                _health = (int) photonView.InstantiationData[1];
            else
                _health = 1;

            _initialHealth = _health;

            TintSprite();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("PlayerBullet"))
                return;

            Destroy(other.gameObject);

            if (!photonView.IsMine) return;

            _killerActorNumber =
                other.gameObject.GetComponent<PlayerBullet>().Owner.ActorNumber;

            photonView.RPC(nameof(LoseHealth), RpcTarget.All);

            if (!IsDead()) return;

            var bulletOwner = other.gameObject.GetComponent<PlayerBullet>().Owner;

            bulletOwner.SetCustomProperties(new Hashtable()
            {
                {PlayerStats.InvaderKills, (int) bulletOwner.CustomProperties[PlayerStats.InvaderKills] + 1}
            });
        }

        #endregion

        [PunRPC]
        private void LoseHealth()
        {
            if (!gameObject || _hidden) return;

            _health--;

            if (IsDead())
            {
                Die();
            }
            else
            {
                if (photonView.IsMine)
                    SoundPlayer.PlaySound(hurtSound);

                TintSprite();
            }
        }

        private void Die()
        {
            if (gameObject.TryGetComponent(out PowerupDrop drop))
                drop.GeneratePowerupDrop();

            SpawnExplosion();

            if (photonView.IsMine)
            {
                SoundPlayer.PlaySound(deathSound);

                PhotonNetwork.CurrentRoom.Players[_killerActorNumber]
                    .AddScore(50 * _initialHealth);

                StartCoroutine(DestroyDelay());
            }

            Hide();
        }

        private void Hide()
        {
            _hidden = true;

            _spriteRenderer.enabled = false;

            GetComponent<Collider2D>().enabled = false;

            if (TryGetComponent(out InvaderShoot shootManager))
                shootManager.StopShooting();
        }

        private IEnumerator DestroyDelay()
        {
            yield return new WaitForSeconds(0.5f);

            PhotonNetwork.Destroy(gameObject);

            if (!PhotonNetwork.CurrentRoom.Players.ContainsKey(_killerActorNumber))
                yield break;

            var killer = PhotonNetwork.CurrentRoom.Players[_killerActorNumber];

            killer.SetCustomProperties(new Hashtable()
            {
                {PlayerStats.InvaderKills, (int) killer.CustomProperties[PlayerStats.InvaderKills] + 1}
            });
        }

        private void TintSprite()
        {
            if (!tintSprite) return;

            var hue = 1.0f - (_health - 1) * 0.25f;
            _spriteRenderer.color = new Color(1.0f, hue, hue);
        }

        private void SpawnExplosion()
        {
            var size = _spriteRenderer.size;

            var centerPoint =
                (Vector2) transform.position + new Vector2(size.x / 2, -size.y / 2);

            Instantiate(explosion, centerPoint, Quaternion.identity);
        }

        private bool IsDead() => _health <= 0;
    }
}