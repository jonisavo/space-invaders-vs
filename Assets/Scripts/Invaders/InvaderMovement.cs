using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace SIVS
{
    public class InvaderMovement : MonoBehaviourPunCallbacks
    {
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
            
            if (!PhotonNetwork.IsMasterClient) return;
            
            StartCoroutine(MoveInvader());
        }

        private void FixedUpdate()
        {
            Debug.DrawRay(
                GetRaycastStartPoint(), 
                GetMovementDirection().normalized * 0.4f,
                Color.red);
        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
                StartCoroutine(MoveInvader());
        }
        
        #endregion

        private IEnumerator MoveInvader()
        {
            while (true)
            {
                yield return new WaitForSeconds(2.0f);

                if (CanMoveInDirection(GetMovementDirection(), 0.4f))
                {
                    Move(GetMovementDirection());
                    continue;
                }
                
                var canMoveDown = CanMoveInDirection(Vector2.down, 2.0f);
                
                foreach (var invader in GameObject.FindGameObjectsWithTag("Invader"))
                {
                    var movement = invader.GetComponent<InvaderMovement>();

                    if (movement._side != _side) continue;
                        
                    movement.ChangeDirection();
                        
                    if (canMoveDown)
                        movement.Move(Vector2.down);
                }
            }
        }

        private void Move(Vector2 direction) =>
            transform.Translate(direction.normalized * 0.25f);

        private void ChangeDirection() => _goingRight = !_goingRight;

        private bool CanMoveHorizontally()
        {
            var hit = Physics2D.Raycast(GetRaycastStartPoint(), GetMovementDirection(),
                0.4f, LayerMask.GetMask("Walls"));

            return hit.collider == null;
        }

        private bool CanMoveInDirection(Vector2 direction, float rayDistance)
        {
            foreach (var invader in GameObject.FindGameObjectsWithTag("Invader"))
            {
                if (invader.GetComponent<InvaderMovement>()._side != _side) continue;

                var hit = Physics2D.Raycast(GetRaycastStartPoint(), direction,
                    rayDistance, LayerMask.GetMask("Walls"));

                if (hit.collider != null) return false;
            }

            return true;
        }

        private Vector2 GetRaycastStartPoint()
        {
            return transform.position + _distanceToCenter;
        }

        private Vector2 GetMovementDirection() => _goingRight ? Vector2.right : Vector2.left;
    }
}