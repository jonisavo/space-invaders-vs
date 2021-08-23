using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine;

namespace SIVS
{
    public class LifePowerup : Powerup
    {
        private const int MaxLifePoints = 500;

        [Tooltip("A popup to show when max lives have been reached.")]
        public GameObject pointsPopup;
        
        protected override void OnPowerupGet(GameObject obj, Player player)
        {
            if (PlayerStats.HasMaximumLives(player))
            {
                ChangeTextPopup(pointsPopup, MaxLifePoints.ToString());
                player.AddScore(MaxLifePoints);
            }
            else
            {
                PlayerStats.AddLife(player);
            }

            base.OnPowerupGet(obj, player);
        }
    }
}