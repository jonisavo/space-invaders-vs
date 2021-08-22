using UnityEngine;
using Photon.Pun;

namespace SIVS
{
    public class PlayerMovement : MonoBehaviourPunCallbacks
    {
        [Tooltip("The movement speed for the player.")]
        public float moveSpeed = 10;

        private Rigidbody2D _rb;

        private void Awake() => _rb = GetComponent<Rigidbody2D>();

        private void FixedUpdate()
        {
            if (!photonView.IsMine || !Match.IsActive)
                return;

            var movementAmount = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;

            _rb.MovePosition((Vector2)transform.position + Vector2.right * movementAmount);
        }
    }
}
