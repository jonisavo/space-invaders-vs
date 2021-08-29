using UnityEngine;

namespace SIVS
{
    public class PlayerBullet : MonoBehaviour
    {
        [Tooltip("The vertical move speed of this bullet.")]
        public float verticalMoveSpeed = 1.0f;

        [Tooltip("The horizontal move speed of this bullet.")]
        public float horizontalMoveSpeed = 1.0f;

        private bool _ownerSet;

        private Rect _ownArea;

        public SIVSPlayer Owner { get; private set; }

        private void Update()
        {
            transform.Translate(
                horizontalMoveSpeed * Time.deltaTime,
                verticalMoveSpeed * Time.deltaTime,
                0);

            if (_ownerSet && OutOfBounds())
                Destroy(gameObject);
        }

        private bool OutOfBounds()
        {
            var position = transform.position;

            return position.y > _ownArea.y + _ownArea.height ||
                   position.y < _ownArea.y ||
                   position.x > _ownArea.x + _ownArea.width ||
                   position.x < _ownArea.x;
        }

        public void SetOwner(SIVSPlayer newOwner)
        {
            Owner = newOwner;

            _ownArea = GameObject.Find("Game Manager")
                .GetComponent<SpawnManager>()
                .PlayAreaRect(newOwner.Number);

            _ownerSet = true;
        }
    }
}
