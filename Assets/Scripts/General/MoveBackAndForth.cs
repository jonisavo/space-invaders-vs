using System.Collections;
using UnityEngine;

namespace SIVS
{
    public class MoveBackAndForth : MonoBehaviour
    {
        [Min(0f)]
        public float distance;

        [Min(0.5f)]
        public float time;

        private float _startX;

        private float _endX;

        private Coroutine _moveCoroutine;

        private void Awake()
        {
            _startX = transform.position.x;
            _endX = _startX + distance;
        }

        private void OnEnable()
        {
            _moveCoroutine = StartCoroutine(MoveCoroutine());
        }

        private void OnDisable()
        {
            StopCoroutine(_moveCoroutine);
            _moveCoroutine = null;
        }

        private IEnumerator MoveCoroutine()
        {
            while (true)
            {
                yield return MoveToX(_endX);
                yield return MoveToX(_startX);
            }
        }

        private IEnumerator MoveToX(float targetX)
        {
            var elapsedTime = 0f;

            var position = transform.position;

            var newPosition = new Vector3(targetX, position.y, position.z);
            
            while (elapsedTime < time)
            {
                transform.position = Vector3.Lerp(position, newPosition, elapsedTime / time);

                yield return null;

                elapsedTime += Time.deltaTime;
            }

            transform.position = newPosition;
        }
    }
}