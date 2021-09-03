using Photon.Pun;
using UnityEngine;

namespace SIVS
{
    [RequireComponent(typeof(PhotonView))]
    public class PowerupOnline : Powerup
    {
        protected PhotonView _photonView;

        protected override void Awake()
        {
            _photonView = GetComponent<PhotonView>();
            base.Awake();
        }

        protected override void OnPlayerEnter(GameObject playerGameObject)
        {
            if (!_photonView.IsMine)
                return;

            base.OnPlayerEnter(playerGameObject);
        }

        protected override SIVSPlayer GetPlayerFromObject(GameObject playerGameObject) =>
            GameManager.Players[playerGameObject.GetPhotonView().Owner.ActorNumber];

        protected override void ObtainPowerup(int playerNumber)
        {
            _photonView.RPC(nameof(ObtainPowerupRPC), RpcTarget.All, playerNumber);
        }

        [PunRPC]
        protected void ObtainPowerupRPC(int playerNumber)
        {
            base.ObtainPowerup(playerNumber);
        }
        
        public override void DestroyPowerup()
        {
            if (_photonView.IsMine)
                PhotonNetwork.Destroy(gameObject);
            else
                if (gameObject)
                    gameObject.SetActive(false);
        }
    }
}