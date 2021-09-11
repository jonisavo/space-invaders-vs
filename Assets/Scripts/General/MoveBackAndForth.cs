using System.Collections;
using UnityEngine;

namespace SIVS
{
    public class MoveBackAndForth : MonoBehaviour
    {
        public float targetX;

        public float targetY;

        [Min(0.5f)]
        public float time;

        private Vector3 _startPosition;

        private Vector3 _endPosition;

        private Coroutine _moveCoroutine;

        private void Awake()
        {
            var position = transform.position;
            
            _startPosition = position;
            _endPosition = new Vector3(targetX, targetY, position.z);
        }

        private void OnEnable()
        {
            _moveCoroutine = StartCoroutine(MoveCoroutine());
        }

        private void OnDisable()
        {
            StopCoroutine(_moveCoroutine);
            _moveCoroutine = null;
            
            transform.position = _startPosition;
        }

        private IEnumerator MoveCoroutine()
        {
            while (true)
            {
                yield return MoveTo(_endPosition);
                yield return MoveTo(_startPosition);
            }
        }

        private IEnumerator MoveTo(Vector3 target)
        {
            var elapsedTime = 0f;

            var startPosition = transform.position;

            while (elapsedTime < time)
            {
                transform.position = Vector3.Lerp(startPosition, target, elapsedTime / time);

                yield return null;

                elapsedTime += Time.deltaTime;
            }

            transform.position = target;
        }
    }
}