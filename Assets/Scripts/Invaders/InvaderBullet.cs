using UnityEngine;

namespace SIVS
{
    public class InvaderBullet : MonoBehaviour
    {
        private const float DespawnHeight = -2.2f;

        [Tooltip("The move speed for this bullet.")]
        public float moveSpeed = 1.0f;

        private void Update()
        {
            transform.Translate(
                0,
                -moveSpeed * Time.deltaTime,
                0);
            if (transform.position.y < DespawnHeight)
                Destroy(gameObject);
        }
    }
}
