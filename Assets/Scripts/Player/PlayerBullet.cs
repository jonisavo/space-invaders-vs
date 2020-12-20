using UnityEngine;

namespace SIVS
{
    public class PlayerBullet : MonoBehaviour
    {
        private const int DespawnHeight = 20;

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

