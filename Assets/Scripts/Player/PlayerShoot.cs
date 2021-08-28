using System.Collections;
using UnityEngine;
using Photon.Pun;

namespace SIVS
{
    [RequireComponent(typeof(AudioSource))]
    public class PlayerShoot : MonoBehaviourPun
    {
        [Tooltip("Bullet(s) that can be shot.")]
        public GameObject[] bulletTypes;

        [Tooltip("The audio clip to play when firing.")]
        public AudioClip fireSound;

        private AudioSource _audioSource;

        private bool _shootingBlockedByOptions;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            OptionsManager.OnOptionsOpen += HandleOptionsOpen;
            OptionsManager.OnOptionsClose += HandleOptionsClose;
        }

        private void OnDisable()
        {
            OptionsManager.OnOptionsOpen -= HandleOptionsOpen;
            OptionsManager.OnOptionsClose -= HandleOptionsClose;
        }
        
        private void Update()
        {
            if (!photonView.IsMine) return;

            if (PressingFireButton() && CanFire())
                photonView.RPC(nameof(FireBullet),
                    RpcTarget.All, PhotonNetwork.LocalPlayer.GetBulletType());
        }

        private bool PressingFireButton() =>
            Input.GetButtonDown("Fire1") || Input.GetButtonDown("Submit");

        private Vector2 GetBulletSpawnPoint()
        {
            var playerTransform = transform;
            return playerTransform.position + playerTransform.forward * 3;
        }

        private bool CanFire() =>
            Match.IsActive && !OwnBulletExists() && !_shootingBlockedByOptions;

        [PunRPC]
        private void FireBullet(PlayerBulletType bulletType)
        {
            if (photonView.IsMine)
                _audioSource.PlayOneShot(fireSound);

            var bulletObj = bulletTypes[(int) bulletType];

            var bulletObject = Instantiate(bulletObj,
                GetBulletSpawnPoint(), Quaternion.identity);

            if (bulletObject.TryGetComponent(out PlayerBullet bullet))
            {
                bullet.SetOwner(Match.GetPlayer(photonView.Owner.ActorNumber));
            }
            else
            {
                foreach (var childBullet in bulletObject.GetComponentsInChildren<PlayerBullet>())
                    childBullet.SetOwner(Match.GetPlayer(photonView.Owner.ActorNumber));
            }
        }

        private bool OwnBulletExists()
        {
            foreach (var bullet in GameObject.FindGameObjectsWithTag("PlayerBullet"))
                if (bullet.GetComponent<PlayerBullet>().Owner.Number == PhotonNetwork.LocalPlayer.ActorNumber)
                    return true;

            return false;
        }

        private void HandleOptionsOpen()
        {
            StopCoroutine(nameof(UnblockShootingCoroutine));
            _shootingBlockedByOptions = true;
        }

        private void HandleOptionsClose() => StartCoroutine(nameof(UnblockShootingCoroutine));

        private IEnumerator UnblockShootingCoroutine()
        {
            yield return new WaitForSeconds(0.25f);

            _shootingBlockedByOptions = false;
        }
    }
}
