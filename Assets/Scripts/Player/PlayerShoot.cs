using UnityEngine;

namespace SIVS
{
    public class PlayerShoot : MonoBehaviour
    {
        public GameObject Bullet;

        private void Update()
        {
            if (Input.GetButtonDown("Fire1"))
                Instantiate(Bullet, GetBulletSpawnPoint(), Quaternion.identity);
        }

        private Vector2 GetBulletSpawnPoint()
        {
            return transform.position + transform.forward * 3;
        }
    }
}