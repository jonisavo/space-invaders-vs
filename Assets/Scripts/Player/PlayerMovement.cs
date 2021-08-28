using UnityEngine;
using Photon.Pun;

namespace SIVS
{
    public class PlayerMovement : MonoBehaviourPun
    {
        [Tooltip("The movement speed for the player.")]
        public float moveSpeed = 10;

        private Rigidbody2D _rb;

        private bool _optionsOpen;

        private void Awake() => _rb = GetComponent<Rigidbody2D>();

        private void OnEnable()
        {
            OptionsManager.OnOptionsOpen += HandleOptionsOpen;
            OptionsManager.OnOptionsClose += HandleOptionsClose;
        }

        private void OnDisable()
        {
            OptionsManager.OnOptionsOpen -= HandleOptionsOpen;
            OptionsManager.OnOptionsClose -= HandleOptionsClose;
        }

        private void FixedUpdate()
        {
            if (!photonView.IsMine || _optionsOpen || !Match.IsActive)
                return;

            var movementAmount = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;

            _rb.MovePosition((Vector2)transform.position + Vector2.right * movementAmount);
        }

        private void HandleOptionsOpen() => _optionsOpen = true;

        private void HandleOptionsClose() => _optionsOpen = false;
    }
}
