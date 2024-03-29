﻿using UnityEngine;

namespace SIVS
{
    public class CoverPowerup : Powerup
    {
        protected override void OnPowerupGet(GameObject obj, SIVSPlayer player)
        {
            AddCover(player.Number, Match.SumOfRounds);

            base.OnPowerupGet(obj, player);
        }
        
        private void AddCover(int playerNumber, int sumOfRounds)
        {
            foreach (var cover in GameObject.FindGameObjectsWithTag("Cover"))
            {
                if (cover.GetComponent<Ownership>().Owner.Number != playerNumber)
                    continue;
                
                cover.GetComponent<Cover>().AddPieces(10 + sumOfRounds * 2);
            }
        }
    }
}