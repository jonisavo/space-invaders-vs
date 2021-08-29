using UnityEngine;

namespace SIVS
{
    public class BulletPowerupOnline : PowerupOnline
    {
        private const int BulletInUsePoints = 250;
        
        [Tooltip("The id of the bullet given by this powerup.")]
        public PlayerBulletType bulletType = PlayerBulletType.Normal;
        
        [Tooltip("A popup to show when the bullet type is already in use.")]
        public GameObject pointsPopup;

        protected override void OnPowerupGet(GameObject obj, SIVSPlayer player)
        {
            if (player.BulletType == bulletType)
            {
                ChangeTextPopup(pointsPopup, BulletInUsePoints.ToString());
                player.AddScore(BulletInUsePoints);
            }
            else
            {
                player.BulletType = bulletType;
            }

            base.OnPowerupGet(obj, player);
        }
    }
}