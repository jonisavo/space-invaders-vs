using UnityEngine;
using Photon.Pun;

namespace SIVS
{
    [RequireComponent(typeof(PhotonView))]
    public class PlayerHealthOnline : PlayerHealth
    {
        private PhotonView _photonView;

        protected override void Awake()
        {
            _photonView = GetComponent<PhotonView>();
            base.Awake();
        }
        
        protected override SIVSPlayer GetThisPlayer() =>
            GameManager.Players[_photonView.Owner.ActorNumber];

        protected override bool ShouldRegisterHit() =>
            _photonView.IsMine && base.ShouldRegisterHit();
        
        protected override void MakeInvincible() =>
            _photonView.RPC(nameof(MakeInvincibleRPC), RpcTarget.All);

        [PunRPC]
        private void MakeInvincibleRPC() => base.MakeInvincible();

        protected override void SpawnExplosion() =>
            _photonView.RPC(nameof(SpawnExplosionRPC), RpcTarget.All);

        [PunRPC]
        private void SpawnExplosionRPC() =>
            base.SpawnExplosion();
    }
}