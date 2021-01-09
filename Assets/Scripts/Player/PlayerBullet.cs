using Photon.Realtime;
using UnityEngine;

namespace SIVS
{
    public class PlayerBullet : MonoBehaviour
    {
        private const float DespawnHeight = 2.4f;

        [Tooltip("The vertical move speed of this bullet.")]
        public float verticalMoveSpeed = 1.0f;

        [Tooltip("The horizontal move speed of this bullet.")]
        public float horizontalMoveSpeed = 1.0f;

        public Player Owner { get; private set; }

        public void SetOwner(Player newOwner) => Owner = newOwner;

        private void Update()
        {
            transform.Translate(
                horizontalMoveSpeed * Time.deltaTime,
                verticalMoveSpeed * Time.deltaTime,
                0);

            if (transform.position.y > DespawnHeight)
                Destroy(gameObject);
        }
    }
}