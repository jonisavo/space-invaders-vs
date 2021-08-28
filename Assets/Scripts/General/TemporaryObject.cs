using UnityEngine;

namespace SIVS
{
    public class TemporaryObject : MonoBehaviour
    {
        [Tooltip("Time in seconds until the object is destroyed.")]
        public float waitTime = 3.0f;

        private void Start()
        {
            Destroy(gameObject, waitTime);
        }
    }
}