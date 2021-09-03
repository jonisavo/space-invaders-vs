using Photon.Pun;
using UnityEngine;

namespace SIVS
{
    [RequireComponent(typeof(PhotonView))]
    public class InvaderHealthOnline : InvaderHealth
    {
        private PhotonView _photonView;

        protected override void Awake()
        {
            _photonView = GetComponent<PhotonView>();
            
            base.Awake();
            
            if (_photonView.InstantiationData != null)
                InitializeHealth((int) _photonView.InstantiationData[1]);
            else
                InitializeHealth(1);
        }
        
        protected override int GetPlayerNumber()
        {
            if (_photonView.InstantiationData != null)
                return (int) _photonView.InstantiationData[0];

            return 1;
        }

        protected override void OnBulletHit(SIVSPlayer player)
        {
            if (!_photonView.IsMine)
                return;
            
            _photonView.RPC(nameof(LoseHealthRPC), RpcTarget.All, player.Number);
        }

        [PunRPC]
        private void LoseHealthRPC(int playerWhoHitNumber) => LoseHealth(playerWhoHitNumber);
        
        protected override void PlayHurtSoundAndShake()
        {
            if (!_photonView.IsMine)
                return;
            
            base.PlayHurtSoundAndShake();
        }
        
        protected override void PlayDeathSoundAndShake()
        {
            if (!_photonView.IsMine)
                return;
            
            base.PlayDeathSoundAndShake();
        }

        protected override void GivePointsToKiller(SIVSPlayer killer, int points)
        {
            if (!_photonView.IsMine)
                return;
            
            base.GivePointsToKiller(killer, points);
        }

        protected override void DestroyObject(SIVSPlayer killer)
        {
            if (!_photonView.IsMine)
                return;
            
            PhotonNetwork.Destroy(gameObject);
            
            if (killer != null)
                killer.InvaderKills += 1;
        }
    }
}