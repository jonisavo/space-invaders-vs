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
        
        protected override void GetHit() =>
            _photonView.RPC(nameof(GetHitRPC), RpcTarget.All);

        [PunRPC]
        private void GetHitRPC() => base.GetHit();
    }
}