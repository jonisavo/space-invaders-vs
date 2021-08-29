using Photon.Pun;
using UnityEngine;

namespace SIVS
{
    [RequireComponent(typeof(PhotonView))]
    public class UFOHealthOnline : UFOHealth
    {
        private PhotonView _photonView;

        private void Awake() => _photonView = GetComponent<PhotonView>();

        protected override void Die(SIVSPlayer killer)
        {
            if (!_photonView.IsMine)
                return;
            
            _photonView.RPC(nameof(DieRPC), RpcTarget.All, killer.Number);
        }

        [PunRPC]
        private void DieRPC(int playerNumber)
        {
            base.Die(GameManager.Players[playerNumber]);
        }

        protected override void ShakeCameraAndStartDestroy()
        {
            if (!_photonView.IsMine)
                return;
            
            base.ShakeCameraAndStartDestroy();
        }

        protected override void DestroyObject() => PhotonNetwork.Destroy(gameObject);
    }
}