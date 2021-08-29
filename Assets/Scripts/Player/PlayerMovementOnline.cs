using UnityEngine;
using Photon.Pun;

namespace SIVS
{
    [RequireComponent(typeof(PhotonView))]
    public class PlayerMovementOnline : PlayerMovement
    {
        private PhotonView _photonView;

        protected override void Awake()
        {
            _photonView = GetComponent<PhotonView>();
            base.Awake();
        }

        protected override string GetInputAxisName() => "Horizontal";

        protected override bool DisallowMovement() =>
            !_photonView.IsMine || base.DisallowMovement();
    }
}
