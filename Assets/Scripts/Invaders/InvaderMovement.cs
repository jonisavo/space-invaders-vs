using UnityEngine;

namespace SIVS
{
    public class InvaderMovement : MonoBehaviour
    {
        [Tooltip("The distance to move the invader each cycle.")]
        public float movementAmount = 0.25f;

        [Tooltip("The layer mask to use for raycasts.")]
        public LayerMask raycastMask;

        private int _playerNumber;

        private bool _goingRight = true;

        private Vector3 _distanceToCenter;

        protected virtual void Awake()
        {
            var bounds = GetComponent<BoxCollider2D>().bounds;
            _distanceToCenter = new Vector3(bounds.size.x / 2, -bounds.size.y / 2, 0);

            _playerNumber = GetPlayerNumber();
        }

        protected virtual int GetPlayerNumber() => GetComponent<Ownership>().Owner.Number;

        public void Move(Vector2 direction) =>
            transform.Translate(direction.normalized *  movementAmount);

        public void ChangeDirection() => _goingRight = !_goingRight;

        public bool CanMoveHorizontally() => CanMoveAll(GetMovementDirection(), movementAmount * 2);

        public bool CanMoveDown() => CanMoveAll(Vector2.down, 2.2f);

        public Vector2 GetMovementDirection() => _goingRight ? Vector2.right : Vector2.left;

        private bool CanMoveAll(Vector2 direction, float rayDistance)
        {
            foreach (var invader in GameObject.FindGameObjectsWithTag("Invader"))
            {
                var movement = invader.GetComponent<InvaderMovementOnline>();

                if (movement._playerNumber != _playerNumber) continue;

                if (!movement.CanMove(direction, rayDistance))
                    return false;
            }

            return true;
        }

        private bool CanMove(Vector2 direction, float rayDistance)
        {
            var hit = Physics2D.Raycast(GetRaycastStartPoint(), direction,
                rayDistance, raycastMask);

            return hit.collider == null;
        }

        private Vector2 GetRaycastStartPoint() => transform.position + _distanceToCenter;
    }
}
