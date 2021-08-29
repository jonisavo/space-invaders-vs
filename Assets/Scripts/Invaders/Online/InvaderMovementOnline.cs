using Photon.Pun;
using UnityEngine;

namespace SIVS
{
    [RequireComponent(typeof(PhotonView))]
    public class InvaderMovementOnline : InvaderMovement
    {
        private PhotonView _photonView;
        
        protected override void Awake()
        {
            _photonView = GetComponent<PhotonView>();
            base.Awake();
        }
        
        protected override int GetPlayerNumber()
        {
            if (_photonView.InstantiationData != null)
                return (int) _photonView.InstantiationData[0];

            return 1;
        }
    }
}
