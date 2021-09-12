using System.Collections;
using RedBlueGames.NotNull;
using UnityEngine;

namespace SIVS
{
    public class ObjectSpawner : MonoBehaviour
    {
        [NotNull]
        public GameObject objectToSpawn;
        
        [Min(0f)]
        public float spawnInterval = 1f;
        
        public Vector3 position = Vector3.zero;
        
        public Vector3 eulerAngles = Vector3.zero;

        private Coroutine _spawnCoroutine;

        private void OnEnable()
        {
            _spawnCoroutine = StartCoroutine(SpawnCoroutine());
        }

        private void OnDisable()
        {
            StopCoroutine(_spawnCoroutine);
            _spawnCoroutine = null;
        }

        private IEnumerator SpawnCoroutine()
        {
            var wait = new WaitForSeconds(spawnInterval);
            
            while (true)
            {
                Instantiate(objectToSpawn, position, Quaternion.Euler(eulerAngles));

                yield return wait;
            }
        }
    }
}