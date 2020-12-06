using UnityEngine;

namespace SIVS
{
    public class PlayerMovement : MonoBehaviour
    {
        public float MoveSpeed = 10;

        private void Update()
        {
            transform.Translate(
                Input.GetAxis("Horizontal") * MoveSpeed * Time.deltaTime,
                0,
                0);
        }
    }
}

