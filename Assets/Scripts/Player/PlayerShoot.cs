using System.Collections;
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

        private AudioSource _audioSource;

        private bool _shootingBlockedByOptions;

        private OptionsManager _optionsManager;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public override void OnEnable()
        {
            OptionsManager.OnOptionsOpen += HandleOptionsOpen;
            OptionsManager.OnOptionsClose += HandleOptionsClose;
        }

        public override void OnDisable()
        {
            OptionsManager.OnOptionsOpen -= HandleOptionsOpen;
            OptionsManager.OnOptionsClose -= HandleOptionsClose;
        }
        
        private void Update()
        {
            if (!photonView.IsMine) return;

            if (PressingFireButton() && CanFire())
                photonView.RPC(nameof(FireBullet),
                    RpcTarget.All, PlayerStats.GetBulletType(PhotonNetwork.LocalPlayer));
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
        private void FireBullet(int bulletType)
        {
            if (photonView.IsMine)
                _audioSource.PlayOneShot(fireSound);

            var bulletObject = Instantiate(bulletTypes[bulletType],
                GetBulletSpawnPoint(), Quaternion.identity);

            if (bulletObject.TryGetComponent(out PlayerBullet bullet))
            {
                bullet.SetOwner(photonView.Owner);
            }
            else
            {
                foreach (var childBullet in bulletObject.GetComponentsInChildren<PlayerBullet>())
                    childBullet.SetOwner(photonView.Owner);
            }
        }

        private bool OwnBulletExists()
        {
            foreach (var bullet in GameObject.FindGameObjectsWithTag("PlayerBullet"))
                if (bullet.GetComponent<PlayerBullet>().Owner.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
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
