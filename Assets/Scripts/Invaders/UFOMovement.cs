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

        private void Awake()
        {
            if (photonView.InstantiationData != null)
                _movementDirection = (bool) photonView.InstantiationData[0] ? Vector2.right : Vector2.left;
            else
                _movementDirection = Vector2.zero;
        }

        private void Update()
        {
            transform.Translate(_movementDirection * (moveSpeed * Time.deltaTime));
            
            if (photonView.IsMine && OutOfBounds())
                PhotonNetwork.Destroy(gameObject);
        }

        private bool OutOfBounds() => 
            Math.Abs(transform.position.x) >= 6 || Math.Abs(transform.position.y) >= 6;
    }
}