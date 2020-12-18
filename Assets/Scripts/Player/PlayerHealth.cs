using UnityEngine;
using Photon.Pun;

namespace SIVS
{
    public class PlayerHealth : MonoBehaviourPunCallbacks
    {
        private int Lives = 3;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!photonView.IsMine) return;

            if (!other.gameObject.CompareTag("EnemyBullet")) return;
            
            GetHit();
        }
        
        private void GetHit()
        {
            photonView.RPC("LoseLife", RpcTarget.AllBuffered);
        }

        [PunRPC]
        private void LoseLife()
        {
            Lives--;
        }
    }
}
