using System.Collections;
using Photon.Pun;
using UnityEngine;

namespace SIVS
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class InvaderShoot : MonoBehaviourPunCallbacks
    {
        [Tooltip("The bullet to shoot.")]
        public GameObject bullet;

        private Vector3 _distanceToShootPoint;

        private float _shootInterval;

        private void Awake()
        {
            var bounds = GetComponent<BoxCollider2D>().bounds;
            _distanceToShootPoint = bounds.min - transform.position;
            _distanceToShootPoint.x += bounds.size.x / 2;
            _distanceToShootPoint.y -= 0.05f;

            if (photonView.InstantiationData != null)
                _shootInterval = (float) photonView.InstantiationData[2];
            else
                _shootInterval = 1.5f;

            if (photonView.IsMine)
                StartCoroutine(ShootCoroutine());
        }

        private IEnumerator ShootCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(_shootInterval);
                
                var hit = Physics2D.Raycast(GetBulletSpawnPoint(), Vector2.down,
                    Mathf.Infinity, LayerMask.GetMask("Invaders"));

                if (hit.collider) continue;
            
                photonView.RPC(nameof(Shoot), RpcTarget.All);
            }
        }

        public void StopShooting()
        {
            if (!photonView.IsMine) return;
            
            StopAllCoroutines();
        }

        private Vector2 GetBulletSpawnPoint()
        {
            return transform.position + _distanceToShootPoint;
        }

        [PunRPC]
        private void Shoot()
        {
            Instantiate(bullet, GetBulletSpawnPoint(), Quaternion.identity);    
        }
    }
}
