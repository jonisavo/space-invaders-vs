using UnityEngine;

namespace SIVS
{
    public class PowerupMovement : MonoBehaviour
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

        protected virtual void DestroyPowerup()
        {
            Destroy(gameObject);
        }
    }
}
