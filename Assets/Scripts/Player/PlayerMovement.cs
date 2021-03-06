﻿using UnityEngine;
using Photon.Pun;
using Vector2 = UnityEngine.Vector2;

namespace SIVS
{
    public class PlayerMovement : MonoBehaviourPunCallbacks
    {
        [Tooltip("The movement speed for the player.")]
        public float moveSpeed = 10;

        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (!photonView.IsMine || !Match.IsActive)
                return;

            _rb.MovePosition((Vector2)transform.position + Vector2.right * (Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime));
        }
    }
}
