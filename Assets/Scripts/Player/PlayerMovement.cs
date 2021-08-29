using UnityEngine;

namespace SIVS
{
    public class PlayerMovement : MonoBehaviour
    {
        [Tooltip("The movement speed for the player.")]
        public float moveSpeed = 10;

        private Rigidbody2D _rb;

        private bool _optionsOpen;

        private string _inputAxisName;

        protected virtual void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _inputAxisName = GetInputAxisName();
        }

        protected virtual string GetInputAxisName()
        {
            var playerNumber = GetComponent<Ownership>().Owner.Number;

            return $"Player {playerNumber} Horizontal";
        }

        protected void OnEnable()
        {
            OptionsManager.OnOptionsOpen += HandleOptionsOpen;
            OptionsManager.OnOptionsClose += HandleOptionsClose;
        }

        protected void OnDisable()
        {
            OptionsManager.OnOptionsOpen -= HandleOptionsOpen;
            OptionsManager.OnOptionsClose -= HandleOptionsClose;
        }

        private void FixedUpdate()
        {
            if (DisallowMovement())
                return;

            var movementAmount = Input.GetAxis(_inputAxisName) * moveSpeed * Time.deltaTime;

            _rb.MovePosition((Vector2)transform.position + Vector2.right * movementAmount);
        }

        protected virtual bool DisallowMovement() => _optionsOpen || !Match.IsActive;

        private void HandleOptionsOpen() => _optionsOpen = true;

        private void HandleOptionsClose() => _optionsOpen = false;
    }
}
