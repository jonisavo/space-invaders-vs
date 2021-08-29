using UnityEngine;

namespace SIVS
{
    public class LifePowerupOnline : PowerupOnline
    {
        private const int MaxLifePoints = 500;

        [Tooltip("A popup to show when max lives have been reached.")]
        public GameObject pointsPopup;
        
        protected override void OnPowerupGet(GameObject obj, SIVSPlayer player)
        {
            if (player.Lives >= SIVSPlayer.MaxLives)
            {
                ChangeTextPopup(pointsPopup, MaxLifePoints.ToString());
                player.AddScore(MaxLifePoints);
            }
            else
            {
                player.AddLife();
            }

            base.OnPowerupGet(obj, player);
        }
    }
}