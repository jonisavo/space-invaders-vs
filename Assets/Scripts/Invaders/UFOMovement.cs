using System;
using Photon.Pun;
using UnityEngine;

namespace SIVS
{
    [RequireComponent(typeof(AudioSource))]
    public class UFOMovement : MonoBehaviourPunCallbacks
    {
        [Tooltip("The movement speed of the UFO.")]
        public float moveSpeed;

        private Vector2 _movementDirection;

        private void Awake()
        {
            var audioSource = GetComponent<AudioSource>();

            if (photonView.InstantiationData != null)
            {
                _movementDirection = (bool) photonView.InstantiationData[0] ? Vector2.right : Vector2.left;
                audioSource.panStereo = (bool) photonView.InstantiationData[0] ? -0.75f : 0.75f;
            }
            else
                _movementDirection = Vector2.zero;
            
            audioSource.Play();
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