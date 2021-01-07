using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;

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
            
            photonView.RPC(nameof(LoseHealth), RpcTarget.All);

            if (!IsDead()) return;

            var bulletOwner = other.gameObject.GetComponent<PlayerBullet>().Owner;
            
            bulletOwner.AddScore(50 * _initialHealth);

            bulletOwner.SetCustomProperties(new Hashtable()
            {
                {PlayerStats.InvaderKills, (int) bulletOwner.CustomProperties[PlayerStats.InvaderKills] + 1}
            });
        }
        
        #endregion

        [PunRPC]
        private void LoseHealth()
        {
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
            if (photonView.IsMine)
            {
                SoundPlayer.PlaySound(deathSound);
                
                PhotonNetwork.Destroy(gameObject);
            }
            else
            {
                if (gameObject) gameObject.SetActive(false);
            }
            
            var size = _spriteRenderer.size;

            var centerPoint =
                (Vector2) transform.position + new Vector2(size.x / 2, -size.y / 2);

            Instantiate(explosion, centerPoint, Quaternion.identity);
        }

        private void TintSprite()
        {
            if (!tintSprite) return;
            
            var hue = 1.0f - (_health - 1) * 0.2f;
            _spriteRenderer.color = new Color(1.0f, hue, hue);
        }

        private bool IsDead() => _health <= 0;
    }   
}
