﻿using Photon.Pun;
using UnityEngine;

namespace SIVS
{
    public class CoverPowerup : Powerup
    {
        protected override void OnPowerupGet(GameObject obj, SIVSPlayer player)
        {
            photonView.RPC(nameof(AddCover),
                RpcTarget.AllBuffered, player.Number, Match.SumOfRounds);
            
            base.OnPowerupGet(obj, player);
        }

        [PunRPC]
        private void AddCover(int actorNumber, int sumOfRounds)
        {
            foreach (var cover in GameObject.FindGameObjectsWithTag("Cover"))
            {
                if (cover.GetPhotonView().Owner.ActorNumber != actorNumber)
                    continue;
                
                cover.GetComponent<CoverOnline>().AddPieces(10 + sumOfRounds * 2);
            }
        }
    }
}