using System.Collections;
using UnityEngine;

namespace SIVS
{
    public class MoveToMonoBehaviour : MonoBehaviour
    {
        [Min(0.5f)]
        public float time;
        
        public float targetX;

        public float targetY;
        
        protected Vector3 StartPosition;

        protected Vector3 EndPosition;
        
        private Coroutine _moveCoroutine;
        
        private void Awake()
        {
            var position = transform.position;
            
            StartPosition = position;
            EndPosition = new Vector3(targetX, targetY, position.z);
        }

        private void OnEnable()
        {
            _moveCoroutine = StartCoroutine(MoveCoroutine());
        }

        private void OnDisable()
        {
            StopCoroutine(_moveCoroutine);
            _moveCoroutine = null;
            
            transform.position = StartPosition;
        }
        
        protected virtual IEnumerator MoveCoroutine()
        {
            yield return MoveTo(EndPosition);
        }
        
        protected IEnumerator MoveTo(Vector3 target)
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