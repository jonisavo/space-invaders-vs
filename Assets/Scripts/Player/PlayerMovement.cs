using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace SIVS
{
    public class PlayerMovement : MonoBehaviour
    {
        public float MoveSpeed = 10;

        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            _rb.MovePosition((Vector2)transform.position + Vector2.right * Input.GetAxis("Horizontal") * MoveSpeed * Time.deltaTime);
            //transform.Translate(
            //    Input.GetAxis("Horizontal") * MoveSpeed * Time.deltaTime,
            //    0,
            //    0);
        }
    }
}

