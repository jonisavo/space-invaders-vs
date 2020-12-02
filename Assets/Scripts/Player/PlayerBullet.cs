using UnityEngine;

namespace SIVS
{
    public class PlayerBullet : MonoBehaviour
    {
        private const int DESPAWN_HEIGHT = 20;

        public float MoveSpeed = 1.0f;

        private void Update()
        {
            transform.Translate(
                0,
                MoveSpeed * Time.deltaTime,
                0);
            if (transform.position.y > DESPAWN_HEIGHT)
                Destroy(gameObject);
        }
    }
}

