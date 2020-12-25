using UnityEngine;
using Photon.Pun;
using Vector2 = UnityEngine.Vector2;

namespace SIVS
{
    public class PlayerMovement : MonoBehaviourPunCallbacks
    {
        public float moveSpeed = 10;

        private Rigidbody2D _rb;
        
        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (!photonView.IsMine) return;

            _rb.MovePosition((Vector2)transform.position + Vector2.right * (Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime));
        }
        
        #endregion
    }
}

