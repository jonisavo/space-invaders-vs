using Photon.Pun;
using UnityEngine;

namespace SIVS
{
    public class PowerupMovement : MonoBehaviourPun
    {
        [Tooltip("Determines how fast the powerup falls.")]
        public float FallSpeed = 1.0f;

        private void Update()
        {
            transform.Translate(Vector3.down * (FallSpeed * Time.deltaTime));
            
            if (transform.position.y < -2.4f)
                DestroyPowerup();
        }

        private void DestroyPowerup()
        {
            if (photonView.IsMine)
                PhotonNetwork.Destroy(gameObject);
            else
                if (gameObject) gameObject.SetActive(false);
        }
    }
}