using Photon.Pun;
using UnityEngine;

namespace SIVS
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class InvaderShoot : MonoBehaviourPunCallbacks
    {
        public GameObject bullet;

        private Vector3 _distanceToShootPoint;

        private int _shootInterval;

        #region MonoBehaviour Callbacks
        
        private void Awake()
        {
            var bounds = GetComponent<BoxCollider2D>().bounds;
            _distanceToShootPoint = bounds.min - transform.position;
            _distanceToShootPoint.x += bounds.size.x / 2;
            _distanceToShootPoint.y -= 0.05f;

            if (photonView.InstantiationData != null)
                _shootInterval = (int) photonView.InstantiationData[1];
            else
                _shootInterval = 200;
        }

        private void FixedUpdate()
        {
            if (!photonView.IsMine) return;
            
            if (Time.frameCount % _shootInterval != 0) return;
            
            var hit = Physics2D.Raycast(GetBulletSpawnPoint(), Vector2.down,
                Mathf.Infinity, LayerMask.GetMask("Invaders"));

            if (hit.collider) return;
            
            photonView.RPC("Shoot", RpcTarget.All);
        }
        
        #endregion

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
