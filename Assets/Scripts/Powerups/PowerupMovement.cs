using Photon.Pun;
using UnityEngine;

namespace SIVS
{
    public class PowerupMovement : MonoBehaviourPun
    {
        private const float DespawnHeight = -2.2f;

        [Tooltip("Determines how fast the powerup falls.")]
        public float fallSpeed = 1.0f;

        private void Update()
        {
            transform.Translate(Vector3.down * (fallSpeed * Time.deltaTime));

            if (transform.position.y < DespawnHeight)
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
