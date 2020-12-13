using UnityEngine;
using Photon.Pun;

namespace SIVS
{
    public class PlayerShoot : MonoBehaviourPunCallbacks
    {
        public GameObject Bullet;

        private void Update()
        {
            if (!photonView.IsMine) return;

            if (Input.GetButtonDown("Fire1"))
                photonView.RPC("FireBullet", RpcTarget.All);
        }

        private Vector2 GetBulletSpawnPoint()
        {
            return transform.position + transform.forward * 3;
        }

        [PunRPC]
        private void FireBullet()
        {
            Instantiate(Bullet, GetBulletSpawnPoint(), Quaternion.identity);
        }
    }
}