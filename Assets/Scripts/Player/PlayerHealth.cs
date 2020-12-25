using UnityEngine;
using Photon.Pun;

namespace SIVS
{
    public class PlayerHealth : MonoBehaviourPunCallbacks
    {
        #region MonoBehaviour Callbacks

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.gameObject.CompareTag("EnemyBullet")) return;
            
            if (photonView.IsMine) GetHit();
            
            Destroy(other.gameObject);
        }
        
        #endregion

        private void GetHit()
        {
            PlayerStats.RemoveLife();
            transform.position = new Vector3(
                PhotonNetwork.LocalPlayer.ActorNumber == 1 ? -2.5f : 2.75f, -1.0f, 0
                );
        }
    }
}
