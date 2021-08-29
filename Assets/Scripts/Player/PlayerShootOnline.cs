using UnityEngine;
using Photon.Pun;

namespace SIVS
{
    [RequireComponent(typeof(PhotonView))]
    public class PlayerShootOnline : PlayerShoot
    {
        private PhotonView _photonView;
        
        protected override void Awake()
        {
            _photonView = GetComponent<PhotonView>();
            base.Awake();
        }

        protected override SIVSPlayer GetPlayerObject() =>
            GameManager.Players[PhotonNetwork.LocalPlayer.ActorNumber];

        protected override bool PressingFireButton() =>
            Input.GetButtonDown("Fire1") || Input.GetButtonDown("Submit");

        protected override bool CanFire() =>
            _photonView.IsMine && base.CanFire();

        protected override void FireBullet(PlayerBulletType bulletType) =>
            _photonView.RPC(nameof(FireBulletRPC), RpcTarget.All, bulletType);

        [PunRPC]
        private void FireBulletRPC(PlayerBulletType bulletType) =>
            base.FireBullet(bulletType);

        protected override void PlayFireSound()
        {
            if (!_photonView.IsMine)
                return;
            
            base.PlayFireSound();
        }
    }
}
