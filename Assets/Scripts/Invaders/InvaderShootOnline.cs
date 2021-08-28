using Photon.Pun;
using UnityEngine;

namespace SIVS
{
    [RequireComponent(typeof(PhotonView))]
    public class InvaderShootOnline : InvaderShoot
    {
        private PhotonView _photonView;
        
        protected override void Awake()
        {
            _photonView = GetComponent<PhotonView>();
            base.Awake();
        }
        
        protected override void StartShooting()
        {
            if (!_photonView.IsMine)
                return;
            
            base.StartShooting();
        }

        public override void StopShooting()
        {
            if (!_photonView.IsMine)
                return;
            
            base.StopShooting();
        }
        
        protected override void Shoot() =>
            _photonView.RPC(nameof(ShootRPC), RpcTarget.All);

        [PunRPC]
        private void ShootRPC() => base.Shoot();

        protected override void PlayShootSound()
        {
            if (!_photonView.IsMine)
                return;
            
            base.PlayShootSound();
        }
    }
}
