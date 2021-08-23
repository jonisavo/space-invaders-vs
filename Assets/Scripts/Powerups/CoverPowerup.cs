using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace SIVS
{
    public class CoverPowerup : Powerup
    {
        protected override void OnPowerupGet(GameObject obj, Player player)
        {
            photonView.RPC(nameof(AddCover),
                RpcTarget.AllBuffered, player.ActorNumber, Match.SumOfRounds);
            
            base.OnPowerupGet(obj, player);
        }

        [PunRPC]
        private void AddCover(int actorNumber, int sumOfRounds)
        {
            foreach (var cover in GameObject.FindGameObjectsWithTag("Cover"))
            {
                if (cover.GetPhotonView().Owner.ActorNumber != actorNumber)
                    continue;
                
                cover.GetComponent<Cover>().AddPieces(10 + sumOfRounds * 2);
            }
        }
    }
}