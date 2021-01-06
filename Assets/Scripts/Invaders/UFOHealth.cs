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
            if (photonView.IsMine)
            {
                GameObject.Find("Sound Player")
                    .GetComponent<AudioSource>()
                    .PlayOneShot(deathSound);
                
                PhotonNetwork.Destroy(gameObject);
            }
            else
            {
                if (gameObject) gameObject.SetActive(false);
            }
            
            Instantiate(explosion, transform.position, Quaternion.identity);
        }
    }
}