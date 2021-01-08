using UnityEngine;
using Photon.Pun;

namespace SIVS
{
    [RequireComponent(typeof(AudioSource))]
    public class PlayerShoot : MonoBehaviourPunCallbacks
    {
        [Tooltip("Bullet(s) that can be shot.")]
        public GameObject[] bulletTypes;

        [Tooltip("The audio clip to play when firing.")]
        public AudioClip fireSound;

        private int _currentBulletType = 0;

        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (!photonView.IsMine) return;

            if (Input.GetButtonDown("Fire1") && CanFire())
                photonView.RPC(nameof(FireBullet), RpcTarget.All);
        }

        public void ChangeBulletType(int id) => _currentBulletType = id;

        private Vector2 GetBulletSpawnPoint()
        {
            var playerTransform = transform;
            return playerTransform.position + playerTransform.forward * 3;
        }

        private bool CanFire() =>
            Match.IsActive && !OwnBulletExists();

        [PunRPC]
        private void FireBullet()
        {
            if (photonView.IsMine)
                _audioSource.PlayOneShot(fireSound);
            
            var bulletObject = Instantiate(bulletTypes[_currentBulletType], 
                GetBulletSpawnPoint(), Quaternion.identity);
            
            bulletObject.GetComponent<PlayerBullet>().SetOwner(photonView.Owner);
        }

        private bool OwnBulletExists()
        {
            foreach (var bullet in GameObject.FindGameObjectsWithTag("PlayerBullet"))
                if (bullet.GetComponent<PlayerBullet>().Owner.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
                    return true;

            return false;
        }
    }
}
