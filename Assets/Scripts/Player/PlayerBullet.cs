using Photon.Realtime;
using UnityEngine;

namespace SIVS
{
    public class PlayerBullet : MonoBehaviour
    {
        private const float DespawnHeight = 2.4f;

        [Tooltip("The move speed of this bullet.")]
        public float moveSpeed = 1.0f;
        
        public Player Owner { get; private set; }

        public void SetOwner(Player newOwner) => Owner = newOwner;

        private void Update()
        {
            transform.Translate(
                0,
                moveSpeed * Time.deltaTime,
                0);
            if (transform.position.y > DespawnHeight)
                Destroy(gameObject);
        }
    }
}
