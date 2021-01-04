using UnityEngine;
using Photon.Pun;

namespace SIVS
{
    public class PlayerShoot : MonoBehaviourPunCallbacks
    {
        [Tooltip("The bullet to shoot.")]
        public GameObject bullet;

        private void Update()
        {
            if (!photonView.IsMine) return;

            if (Input.GetButtonDown("Fire1") && CanFire())
                photonView.RPC("FireBullet", RpcTarget.All);
        }

        private Vector2 GetBulletSpawnPoint()
        {
            var playerTransform = transform;
            return playerTransform.position + playerTransform.forward * 3;
        }

        private bool CanFire() =>
            Match.IsActive && !OwnBulletExists();

        [PunRPC]
        private void FireBullet()
        {
            var bulletObject = Instantiate(bullet, GetBulletSpawnPoint(), Quaternion.identity);
            bulletObject.GetComponent<PlayerBullet>().SetOwner(photonView.Owner);
        }

        private bool OwnBulletExists()
        {
            foreach (var bullet in GameObject.FindGameObjectsWithTag("PlayerBullet"))
                if (bullet.GetComponent<PlayerBullet>().Owner.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
                    return true;

            return false;
        }
    }
}
