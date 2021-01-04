using UnityEngine;

namespace SIVS
{
    public class UFOMovement : MonoBehaviour
    {
        [Tooltip("The movement speed of the UFO.")]
        public float moveSpeed;

        private Vector2 _movementDirection;

        #region MonoBehaviour Callbacks
        
        private void Awake()
        {
            _movementDirection = Vector2.zero;
        }

        private void Update()
        {
            transform.Translate(_movementDirection * (moveSpeed * Time.deltaTime));
        }
        
        #endregion

        public void SetMovementDirection(Vector2 direction) => _movementDirection = direction;
    }
}