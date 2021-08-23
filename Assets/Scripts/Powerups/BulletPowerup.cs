using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine;

namespace SIVS
{
    public class BulletPowerup : Powerup
    {
        private const int BulletInUsePoints = 250;
        
        [Tooltip("The id of the bullet given by this powerup.")]
        public int bulletType;
        
        [Tooltip("A popup to show when the bullet type is already in use.")]
        public GameObject pointsPopup;

        protected override void OnPowerupGet(GameObject obj, Player player)
        {
            if (PlayerStats.GetBulletType(player) == bulletType)
            {
                ChangeTextPopup(pointsPopup, BulletInUsePoints.ToString());
                player.AddScore(BulletInUsePoints);
            }
            else
            {
                PlayerStats.ChangeBulletType(player, bulletType);
            }

            base.OnPowerupGet(obj, player);
        }
    }
}