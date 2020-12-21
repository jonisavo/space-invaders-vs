using UnityEngine;

namespace SIVS
{
    public class PlayerBullet : MonoBehaviour
    {
        private const float DespawnHeight = 2.4f;

        public float moveSpeed = 1.0f;

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

