using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;

namespace SIVS
{
    public class UFOHealth : MonoBehaviourPunCallbacks
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("PlayerBullet")) return;
            
            Destroy(other.gameObject);
            
            if (!photonView.IsMine) return;
            
            var bulletOwner = other.gameObject.GetComponent<PlayerBullet>().Owner;
            
            bulletOwner.AddScore(1000);
            
            photonView.RPC("Die", RpcTarget.All);
        }

        [PunRPC]
        private void Die()
        {
            if (photonView.IsMine)
                PhotonNetwork.Destroy(gameObject);
            else
                if (gameObject) gameObject.SetActive(false);
        }
    }
}