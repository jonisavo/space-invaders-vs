using UnityEngine;
using Photon.Pun;
using Random = UnityEngine.Random;

namespace SIVS
{
    [RequireComponent(typeof(PhotonView))]
    public class InvaderManagerOnline : InvaderManager
    {
        private PhotonView _photonView;

        protected override void Awake()
        {
            _photonView = GetComponent<PhotonView>();
            base.Awake();
        }

        protected override void SpawnUFOForPlayer(SIVSPlayer player)
        {
            var spawnXCoord = player.Number == 1 ? -3.0f : 3.0f;
            var position = _spawnManager.PlayAreaPosition(player.Number, spawnXCoord, 2.0f);

            // TODO: Is this necessary? Should we switch to the Ownership component entirely?
            object[] instantiationData = { player.Number == 1 };

            PhotonNetwork.Instantiate("UFO",
                position, Quaternion.identity, 0, instantiationData);
                
            _photonView.RPC(nameof(ShowUfoSpawnedPopupRPC), RpcTarget.All);
        }

        [PunRPC]
        private void ShowUfoSpawnedPopupRPC()
        {
            var popupObject = Instantiate(textPopupObject, Vector3.zero, Quaternion.identity);
            
            popupObject.GetComponent<TextPopup>().Show("<animation=verticalpos>GET THAT UFO!</animation>");
        }

        public override void InitializeAllInvaders()
        {
            InitializeInvadersForPlayer(GameManager.Players[PhotonNetwork.LocalPlayer.ActorNumber]);
        }

        protected override void SpawnInvadersForPlayer(SIVSPlayer player)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber != player.Number)
                return;
            
            base.SpawnInvadersForPlayer(player);
        }

        protected override void SpawnOneInvaderForPlayer(SIVSPlayer player, int row, int column)
        {
            object[] instantiationData = {player.Number, GenerateInvaderHealth(), Random.Range(3.0f, 4.75f)};

            var position = _spawnManager.PlayAreaPosition(
                player.Number, -1.75f + row * 0.4f, 2.1f - column * 0.3f
            );

            PhotonNetwork.Instantiate("Invader",
                position, Quaternion.identity, 0, instantiationData);
        }

        protected override bool InvaderOwnedByPlayer(GameObject invader, SIVSPlayer player)
        {
            return invader.GetPhotonView().Owner.ActorNumber == player.Number;
        }
    }
}
