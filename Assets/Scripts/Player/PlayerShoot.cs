using System.Collections;
using RedBlueGames.NotNull;
using UnityEngine;

namespace SIVS
{
    [RequireComponent(typeof(AudioSource))]
    public class PlayerShoot : MonoBehaviour
    {
        [Tooltip("Bullet(s) that can be shot.")]
        [NotNull]
        public GameObject[] bulletTypes;

        [Tooltip("The audio clip to play when firing.")]
        [NotNull]
        public AudioClip fireSound;

        private AudioSource _audioSource;

        private bool _shootingBlockedByOptions;

        private SIVSPlayer _player;

        protected virtual void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _player = GetPlayerObject();
        }

        protected virtual SIVSPlayer GetPlayerObject() => GetComponent<Ownership>().Owner;

        protected void OnEnable()
        {
            OptionsManager.OnOptionsOpen += HandleOptionsOpen;
            OptionsManager.OnOptionsClose += HandleOptionsClose;
        }

        protected void OnDisable()
        {
            OptionsManager.OnOptionsOpen -= HandleOptionsOpen;
            OptionsManager.OnOptionsClose -= HandleOptionsClose;
        }
        
        private void Update()
        {
            if (PressingFireButton() && CanFire())
                FireBullet(_player.BulletType);
        }

        protected virtual bool PressingFireButton() =>
            Input.GetButtonDown($"Player {_player.Number} Fire");

        private Vector2 GetBulletSpawnPoint()
        {
            var playerTransform = transform;
            return playerTransform.position + playerTransform.forward * 3;
        }

        protected virtual bool CanFire() =>
            Match.IsActive && !OwnBulletExists() && !_shootingBlockedByOptions;
        
        protected virtual void FireBullet(PlayerBulletType bulletType)
        {
            PlayFireSound();

            var bulletObj = bulletTypes[(int) bulletType];

            var bulletObject = Instantiate(bulletObj,
                GetBulletSpawnPoint(), Quaternion.identity);

            if (bulletObject.TryGetComponent(out PlayerBullet bullet))
            {
                bullet.SetOwner(_player);
            }
            else
            {
                foreach (var childBullet in bulletObject.GetComponentsInChildren<PlayerBullet>())
                    childBullet.SetOwner(_player);
            }
        }
        
        protected virtual void PlayFireSound() => _audioSource.PlayOneShot(fireSound);

        private bool OwnBulletExists()
        {
            foreach (var bullet in GameObject.FindGameObjectsWithTag("PlayerBullet"))
                if (bullet.GetComponent<PlayerBullet>().Owner.Number == _player.Number)
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
