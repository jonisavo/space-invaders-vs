using Photon.Pun;
using UnityEngine;

namespace SIVS
{
    public class InvaderMovement : MonoBehaviourPunCallbacks
    {
        [Tooltip("The distance to move the invader each cycle.")]
        public float movementAmount = 0.25f;
        
        [Tooltip("Toggles the drawing of a debug ray.")]
        public bool drawDebugRay = false;
        
        private int _side;

        private bool _goingRight = true;

        private Vector3 _distanceToCenter;

        #region Unity Callbacks

        private void Awake()
        {
            var bounds = GetComponent<BoxCollider2D>().bounds;
            _distanceToCenter = new Vector3(bounds.size.x / 2, -bounds.size.y / 2, 0);

            if (photonView.InstantiationData != null)
                _side = (int) photonView.InstantiationData[0];
            else
                _side = 1;
        }

        private void FixedUpdate()
        {
            if (!drawDebugRay) return;
            
            Debug.DrawRay(
                GetRaycastStartPoint(), 
                GetMovementDirection().normalized * (movementAmount * 2),
                Color.red);
        }

        #endregion

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
                var movement = invader.GetComponent<InvaderMovement>();
                
                if (movement._side != _side) continue;

                if (!movement.CanMove(direction, rayDistance))
                    return false;
            }

            return true;
        }

        private bool CanMove(Vector2 direction, float rayDistance)
        {
            var hit = Physics2D.Raycast(GetRaycastStartPoint(), direction,
                rayDistance, LayerMask.GetMask("Walls"));

            return hit.collider == null;
        }

        private Vector2 GetRaycastStartPoint() => transform.position + _distanceToCenter;
    }
}
