using Photon.Pun;
using UnityEngine;

namespace SIVS
{
    [RequireComponent(typeof(PhotonView))]
    public class UFOMovementOnline : UFOMovement
    {
        private PhotonView _photonView;

        protected override void Awake()
        {
            _photonView = GetComponent<PhotonView>();
            base.Awake();
        }

        protected override Vector2 GetMovementDirection()
        {
            if (_photonView.InstantiationData != null)
                return (bool) _photonView.InstantiationData[0] ? Vector2.right : Vector2.left;
            
            return Vector2.zero;
        }

        protected override void PlayPannedEntrySound()
        {
            if (_photonView.InstantiationData != null)
                _audioSource.panStereo = (bool) _photonView.InstantiationData[0] ? -0.75f : 0.75f;
            
            _audioSource.Play();
        }

        protected override void HandleMidwayCross()
        {
            if (!_photonView.IsMine)
                return;
            
            var playerNumber = _movementDirection == Vector2.right ? 2 : 1;
            
            _photonView.TransferOwnership(playerNumber);
        }

        protected override bool ShouldDestroy() => _photonView.IsMine && base.ShouldDestroy();

        protected override void DestroyObject() => PhotonNetwork.Destroy(gameObject);
    }
}