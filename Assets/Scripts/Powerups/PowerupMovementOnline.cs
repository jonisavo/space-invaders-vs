using Photon.Pun;
using UnityEngine;

namespace SIVS
{
    [RequireComponent(typeof(PhotonView))]
    public class PowerupMovementOnline : PowerupMovement
    {
        private PhotonView _photonView;

        private void Awake() => _photonView = GetComponent<PhotonView>();

        protected override void DestroyPowerup()
        {
            if (_photonView.IsMine)
                PhotonNetwork.Destroy(gameObject);
            else
                if (gameObject) gameObject.SetActive(false);
        }
    }
}
