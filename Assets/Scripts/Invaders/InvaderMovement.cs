using Photon.Pun;
using UnityEngine;

namespace SIVS
{
    public class InvaderMovement : MonoBehaviourPunCallbacks
    {
        public float movementAmount = 0.25f;
        
        public bool debugMode = false;
        
        private int _side;

        private bool _goingRight = true;

        private Vector3 _distanceToCenter;
        
        #region Callbacks

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
            if (!debugMode) return;
            
            Debug.DrawRay(
                GetRaycastStartPoint(), 
                GetMovementDirection().normalized * 0.5f,
                Color.red);
        }

        #endregion

        public int GetSide() => _side;

        public void Move(Vector2 direction) =>
            transform.Translate(direction.normalized * movementAmount);

        public void ChangeDirection() => _goingRight = !_goingRight;

        public bool CanMoveHorizontally() => CanMove(GetMovementDirection(), movementAmount * 2);

        public bool CanMoveDown() => CanMove(Vector2.down, 2.5f);

        private bool CanMove(Vector2 direction, float rayDistance)
        {
            foreach (var invader in GameObject.FindGameObjectsWithTag("Invader"))
            {
                if (invader.GetComponent<InvaderMovement>()._side != _side) continue;

                var hit = Physics2D.Raycast(GetRaycastStartPoint(), direction,
                    rayDistance, LayerMask.GetMask("Walls"));

                if (hit.collider) return false;
            }

            return true;
        }
        
        public Vector2 GetMovementDirection() => _goingRight ? Vector2.right : Vector2.left;

        private Vector2 GetRaycastStartPoint() => transform.position + _distanceToCenter;
    }
}