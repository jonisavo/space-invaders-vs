using Photon.Pun;
using UnityEngine;

namespace SIVS
{
    public class CoverPowerupOnline : PowerupOnline
    {
        protected override void OnPowerupGet(GameObject obj, SIVSPlayer player)
        {
            _photonView.RPC(nameof(AddCover),
                RpcTarget.AllBuffered, player.Number, Match.SumOfRounds);
            
            base.OnPowerupGet(obj, player);
        }

        [PunRPC]
        private void AddCover(int playerNumber, int sumOfRounds)
        {
            foreach (var cover in GameObject.FindGameObjectsWithTag("Cover"))
            {
                if (cover.GetPhotonView().Owner.ActorNumber != playerNumber)
                    continue;
                
                cover.GetComponent<CoverOnline>().AddPieces(10 + sumOfRounds * 2);
            }
        }
    }
}