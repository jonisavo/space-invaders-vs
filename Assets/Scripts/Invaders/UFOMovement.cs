using System;
using UnityEngine;

namespace SIVS
{
    [RequireComponent(typeof(AudioSource))]
    public class UFOMovement : MonoBehaviour
    {
        [Tooltip("The movement speed of the UFO.")]
        public float moveSpeed;

        protected Vector2 _movementDirection;

        protected AudioSource _audioSource;

        protected virtual void Awake() => _audioSource = GetComponent<AudioSource>();

        private void Start()
        {
            _movementDirection = GetMovementDirection();
            
            PlayPannedEntrySound();
        }

        protected virtual Vector2 GetMovementDirection()
        {
            var playerNumber = GetComponent<Ownership>().Owner.Number;
            return playerNumber == 1 ? Vector2.right : Vector2.left;
        }

        protected virtual void PlayPannedEntrySound()
        {
            var playerNumber = GetComponent<Ownership>().Owner.Number;
            _audioSource.panStereo = playerNumber == 1 ? -0.75f : 0.75f;
            _audioSource.Play();
        }

        private void Update()
        {
            var preTranslateXPosition = transform.position.x;
            
            transform.Translate(_movementDirection * (moveSpeed * Time.deltaTime));
            
            if (CrossedMidwayPoint(preTranslateXPosition))
                HandleMidwayCross();
            
            if (ShouldDestroy())
                DestroyObject();
        }

        private bool CrossedMidwayPoint(float preTranslateXPosition)
        {
            if (_movementDirection == Vector2.right)
                return preTranslateXPosition <= 0 && transform.position.x > 0;

            return preTranslateXPosition >= 0 && transform.position.x < 0;
        }
        
        protected virtual void HandleMidwayCross() {}

        protected virtual bool ShouldDestroy() => OutOfBounds();

        protected virtual void DestroyObject() => Destroy(gameObject);

        private bool OutOfBounds() => 
            Math.Abs(transform.position.x) >= 6 || Math.Abs(transform.position.y) >= 6;
    }
}