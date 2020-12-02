using UnityEngine;

namespace SIVS
{
    public class PlayerShoot : MonoBehaviour
    {
        public GameObject bullet;

        // Update is called once per frame
        void Update()
        {
            if (Input.GetButtonDown("Fire1"))
                Instantiate(bullet, transform.position + transform.forward * 3, Quaternion.identity);
        }
    }
}