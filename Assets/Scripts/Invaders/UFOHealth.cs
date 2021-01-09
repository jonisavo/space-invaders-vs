using System.Collections;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;

namespace SIVS
{
    [RequireComponent(typeof(AudioSource))]
    public class UFOHealth : MonoBehaviourPunCallbacks
    {
        [Tooltip("GameObject to instantiate as the UFO's explosion.")]
        public GameObject explosion;

        [Tooltip("Audio clip to play upon death.")]
        public AudioClip deathSound;

        private bool _hidden;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("PlayerBullet"))
                return;

            Destroy(other.gameObject);

            if (!photonView.IsMine) return;

            var bulletOwner = other.gameObject.GetComponent<PlayerBullet>().Owner;

            bulletOwner.AddScore(1000);

            photonView.RPC(nameof(Die), RpcTarget.All);
        }

        [PunRPC]
        private void Die()
        {
            if (_hidden) return;

            SoundPlayer.PlaySound(deathSound);

            if (gameObject.TryGetComponent(out PowerupDrop drop))
                drop.GeneratePowerupDrop();

            if (photonView.IsMine)
                StartCoroutine(DestroyDelay());

            Hide();

            Instantiate(explosion, transform.position, Quaternion.identity);
        }

        private IEnumerator DestroyDelay()
        {
            yield return new WaitForSeconds(0.5f);

            PhotonNetwork.Destroy(gameObject);
        }

        private void Hide()
        {
            _hidden = true;

            GetComponent<SpriteRenderer>().enabled = false;

            GetComponent<Collider2D>().enabled = false;
        }
    }
}