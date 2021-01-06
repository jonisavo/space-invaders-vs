using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;

namespace SIVS
{
    public class LifePowerup : MonoBehaviourPun
    {
        [Tooltip("Sound effect played when the powerup is obtained.")]
        public AudioClip soundEffect;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player"))
                return;

            var player = other.gameObject.GetPhotonView().Owner;

            if (PhotonNetwork.LocalPlayer.ActorNumber != player.ActorNumber)
                return;
            
            if (PlayerStats.HasMaximumLives(player))
                player.AddScore(500);
            else
                PlayerStats.AddLife(player);
            
            GameObject.Find("Sound Player")
                .GetComponent<AudioSource>()
                .PlayOneShot(soundEffect);
            
            photonView.RPC(nameof(DestroyPowerup), RpcTarget.All);
        }

        [PunRPC]
        private void DestroyPowerup()
        {
            if (photonView.IsMine)
                PhotonNetwork.Destroy(gameObject);
            else
                if (gameObject) gameObject.SetActive(false);
        }
    }
}