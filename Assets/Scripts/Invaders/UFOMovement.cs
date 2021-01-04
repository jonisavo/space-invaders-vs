using System;
using Photon.Pun;
using UnityEngine;

namespace SIVS
{
    public class UFOMovement : MonoBehaviourPunCallbacks
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
            
            if (photonView.IsMine && OutOfBounds())
                PhotonNetwork.Destroy(gameObject);
        }
        
        #endregion

        public void SetMovementDirection(Vector2 direction) => _movementDirection = direction;

        private bool OutOfBounds() => 
            Math.Abs(transform.position.x) >= 6 || Math.Abs(transform.position.y) >= 6;
    }
}