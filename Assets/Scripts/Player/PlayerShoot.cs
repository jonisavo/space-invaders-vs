﻿using UnityEngine;
using Photon.Pun;

namespace SIVS
{
    public class PlayerShoot : MonoBehaviourPunCallbacks
    {
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

        private bool CanFire()
        {
            return GameObject.FindWithTag("PlayerBullet") == null;
        }

        [PunRPC]
        private void FireBullet()
        {
            Instantiate(bullet, GetBulletSpawnPoint(), Quaternion.identity);
        }
    }
}