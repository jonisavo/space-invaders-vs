using UnityEngine;
using Photon.Pun;

namespace SIVS
{
    public class PlayerHealth : MonoBehaviourPunCallbacks
    {
        private GameStatistics _statistics;

        private void Start()
        {
            _statistics = GameObject.Find("Game Manager").GetComponent<GameStatistics>();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!photonView.IsMine) return;

            if (!other.gameObject.CompareTag("EnemyBullet")) return;
            
            Debug.Log("Got hit");
            
            GetHit();
        }

        private void GetHit()
        {
            photonView.RPC("LoseLife", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.NickName);
            transform.position = new Vector3(
                PhotonNetwork.LocalPlayer.ActorNumber == 1 ? -2.5f : 2.75f, -1.0f, 0
                );
        }

        [PunRPC]
        private void LoseLife(string nickName)
        {
            Debug.Log("Decreasing one life from " + nickName);
            _statistics.GetStatistics(nickName).Lives--;
        }
    }
}
