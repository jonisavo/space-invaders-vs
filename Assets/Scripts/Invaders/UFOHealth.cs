using System.Collections;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;

namespace SIVS
{
    [RequireComponent(typeof(AudioSource))]
    public class UFOHealth : MonoBehaviourPunCallbacks
    {
        private const int KillPoints = 1000;

        [Tooltip("GameObject to instantiate as the UFO's explosion.")]
        public GameObject explosion;

        [Tooltip("Audio clip to play upon death.")]
        public AudioClip deathSound;

        [Tooltip("GameObject containing a TextPopup component to instantiate when the UFO is destroyed.")]
        public GameObject pointsObject;
        
        public delegate void OnKillDelegate(int killerActorNumber);

        public static event OnKillDelegate OnKill;

        private bool _hidden;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("PlayerBullet"))
                return;

            Destroy(other.gameObject);

            if (!photonView.IsMine) return;

            var bulletOwner = other.gameObject.GetComponent<PlayerBullet>().Owner;

            bulletOwner.AddScore(KillPoints);

            photonView.RPC(nameof(Die), RpcTarget.All, bulletOwner.ActorNumber);
        }

        [PunRPC]
        private void Die(int actorNumber)
        {
            if (_hidden) return;

            SoundPlayer.PlaySound(deathSound);
            
            OnKill?.Invoke(actorNumber);

            var currentPosition = transform.position;
            
            var pointsObj = Instantiate(pointsObject, currentPosition, Quaternion.identity);
            pointsObj.GetComponent<TextPopup>().Show($"<animation=slowsine>{KillPoints}</animation>");

            if (gameObject.TryGetComponent(out PowerupDrop drop))
                drop.GeneratePowerupDrop();

            if (photonView.IsMine)
            {
                StartCoroutine(DestroyDelay());
                CameraShaker.ShakeAll(0.1f, 0.35f);
            }

            Hide();

            Instantiate(explosion, currentPosition, Quaternion.identity);
            Instantiate(explosionParticles, currentPosition, Quaternion.identity);
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