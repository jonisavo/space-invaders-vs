﻿using System.Collections;
using UnityEngine;
using Photon.Pun;

namespace SIVS
{
    [RequireComponent(typeof(AudioSource))]
    public class PlayerHealth : MonoBehaviourPunCallbacks
    {
        [Tooltip("GameObject to instantiate as the player's explosion.")]
        public GameObject explosion;

        [Tooltip("Audio clip to play upon explosion.")]
        public AudioClip explosionSound;

        [Tooltip("The player's sprite renderer.")]
        public SpriteRenderer spriteRenderer;

        [Tooltip("A debug option to make players invincible.")]
        public bool invincibility;

        public delegate void OnHitDelegate(GameObject playerObject);

        public static event OnHitDelegate OnSelfHit;

        private AudioSource _audioSource;

        private bool _invincibilityFrames;

        #region Unity Callbacks

        private void Awake() => _audioSource = GetComponent<AudioSource>();

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (invincibility) return;

            if (!other.gameObject.CompareTag("EnemyBullet"))
                return;

            Destroy(other.gameObject);

            if (photonView.IsMine && !_invincibilityFrames)
                GetHit();
        }

        #endregion

        [PunRPC]
        private void MakeInvincible()
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

            photonView.RPC(nameof(SpawnExplosion), RpcTarget.All);

            photonView.RPC(nameof(MakeInvincible), RpcTarget.All);
            
            PhotonNetwork.LocalPlayer.SetBulletType(PlayerBulletType.Normal);

            OnSelfHit?.Invoke(gameObject);
            
            PhotonNetwork.LocalPlayer.RemoveLife();
        }

        [PunRPC]
        private void SpawnExplosion() =>
            Instantiate(explosion, transform.position, Quaternion.identity);
    }
}