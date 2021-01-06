using Photon.Pun;
using UnityEngine;

namespace SIVS
{
    public class CoverPowerup : MonoBehaviourPun
    {
        [Tooltip("Sound effect played when the powerup is obtained.")]
        public AudioClip soundEffect;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player"))
                return;

            var player = other.gameObject.GetPhotonView().Owner;

            if (PhotonNetwork.LocalPlayer.ActorNumber != player.ActorNumber)
                return;
            
            photonView.RPC(nameof(AddCover), RpcTarget.AllBuffered, player.ActorNumber);
            
            GameObject.Find("Sound Player")
                .GetComponent<AudioSource>()
                .PlayOneShot(soundEffect);
            
            photonView.RPC(nameof(DestroyPowerup), RpcTarget.All);
        }

        [PunRPC]
        private void DestroyPowerup()
        {
            if (photonView.IsMine)
                PhotonNetwork.Destroy(gameObject);
            else
            if (gameObject) gameObject.SetActive(false);
        }
        
        [PunRPC]
        private void AddCover(int actorNumber)
        {
            foreach (var cover in GameObject.FindGameObjectsWithTag("Cover"))
            {
                if (cover.GetPhotonView().Owner.ActorNumber != actorNumber)
                    continue;
                
                cover.GetComponent<Cover>().AddPieces(10);
            }
        }
    }
}