using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace SIVS
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class InvaderShoot : MonoBehaviourPunCallbacks
    {
        public GameObject bullet;

        private Vector3 _distanceToShootPoint;

        private float _shootInterval;

        #region MonoBehaviour Callbacks
        
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

            if (PhotonNetwork.IsMasterClient)
                StartCoroutine(ShootCoroutine());
        }

        // private void FixedUpdate()
        // {
        //     if (!photonView.IsMine) return;
        //     
        //     if (Time.frameCount % _shootInterval != 0) return;
        //     
        //     var hit = Physics2D.Raycast(GetBulletSpawnPoint(), Vector2.down,
        //         Mathf.Infinity, LayerMask.GetMask("Invaders"));
        //
        //     if (hit.collider) return;
        //     
        //     photonView.RPC("Shoot", RpcTarget.All);
        // }
        
        #endregion
        
        #region MonoBehaviourPunCallbacks Callbacks

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
                StartCoroutine(ShootCoroutine());
        }

        #endregion

        private IEnumerator ShootCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(_shootInterval);
                
                var hit = Physics2D.Raycast(GetBulletSpawnPoint(), Vector2.down,
                    Mathf.Infinity, LayerMask.GetMask("Invaders"));

                if (hit.collider) continue;
            
                photonView.RPC("Shoot", RpcTarget.All);
            }
        }

        public void StopShooting()
        {
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
