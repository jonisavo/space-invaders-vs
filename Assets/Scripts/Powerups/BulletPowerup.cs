using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;

namespace SIVS
{
    public class BulletPowerup : MonoBehaviourPun
    {
        [Tooltip("The id of the bullet given by this powerup.")]
        public int bulletType = 0;

        [Tooltip("Sound effect played when the powerup is obtained.")]
        public AudioClip soundEffect;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player"))
                return;

            var player = other.gameObject.GetPhotonView().Owner;

            if (PhotonNetwork.LocalPlayer.ActorNumber != player.ActorNumber)
                return;

            if (PlayerStats.GetBulletType(player) == bulletType)
                player.AddScore(250);
            else
                PlayerStats.ChangeBulletType(player, bulletType);

            SoundPlayer.PlaySound(soundEffect);

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