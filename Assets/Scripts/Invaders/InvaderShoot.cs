using UnityEngine;

namespace SIVS
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class InvaderShoot : MonoBehaviour
    {
        public GameObject bullet;

        private Vector3 _distanceToShootPoint;

        private int _shootInterval;

        private void Awake()
        {
            var bounds = GetComponent<BoxCollider2D>().bounds;
            _distanceToShootPoint = bounds.min - transform.position;
            _distanceToShootPoint.x += bounds.size.x / 2;
            _distanceToShootPoint.y -= 0.05f;
        }

        private void Start()
        {
            _shootInterval = GameObject.Find("Game Manager").GetComponent<GameRandomizer>().GetInt(180, 300);
        }

        private void FixedUpdate()
        {
            if (Time.frameCount % _shootInterval != 0) return;
            
            var hit = Physics2D.Raycast(GetBulletSpawnPoint(), Vector2.down,
                Mathf.Infinity, LayerMask.GetMask("Invaders"));

            if (hit.collider) return;
            
            Shoot();
        }

        private Vector2 GetBulletSpawnPoint()
        {
            return transform.position + _distanceToShootPoint;
        }

        private void Shoot()
        {
            Instantiate(bullet, GetBulletSpawnPoint(), Quaternion.identity);    
        }
    }
}
