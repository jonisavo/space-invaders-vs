using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

namespace SIVS
{
    public class SIVSPhotonPlayer : SIVSPlayer, IInRoomCallbacks
    {
        public override int Score => _photonPlayer.GetScore();

        public override bool Ready
        {
            get => _photonPlayer.IsReady();
            set => _photonPlayer.SetReady(value);
        }

        public override int CurrentRound
        {
            get => _photonPlayer.GetRound();
            set => _photonPlayer.SetRound(value);
        }

        public override int Lives
        {
            get => _photonPlayer.GetLives();
            set => _photonPlayer.SetLives(value);
        }

        public override int InvaderKills
        {
            get => _photonPlayer.GetInvaderKills();
            set => _photonPlayer.SetInvaderKills(value);
        }

        public override PlayerBulletType BulletType
        {
            get => _photonPlayer.GetBulletType();
            set => _photonPlayer.SetBulletType(value);
        }

        private readonly Player _photonPlayer;

        public SIVSPhotonPlayer(Player photonPlayer) : base(photonPlayer.NickName, photonPlayer.ActorNumber)
        {
            _photonPlayer = photonPlayer;
            PhotonNetwork.AddCallbackTarget(this);
        }

        ~SIVSPhotonPlayer()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }
        
        #region PUN Callbacks

        public void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if (targetPlayer.ActorNumber != _photonPlayer.ActorNumber)
                return;

            if (changedProps.ContainsKey(PlayerPhotonPropertyKey.Lives))
                EmitLivesChangeEvent((int) changedProps[PlayerPhotonPropertyKey.Lives]);
            
            if (changedProps.ContainsKey(PlayerPhotonPropertyKey.Score))
                EmitScoreChangeEvent((int) changedProps[PlayerPhotonPropertyKey.Score]);
            
            if (changedProps.ContainsKey(PlayerPhotonPropertyKey.Ready))
                EmitReadyChangeEvent((bool) changedProps[PlayerPhotonPropertyKey.Ready]);

            if (changedProps.ContainsKey(PlayerPhotonPropertyKey.BulletType))
                EmitBulletTypeChangeEvent((PlayerBulletType) changedProps[PlayerPhotonPropertyKey.BulletType]);
            
            if (changedProps.ContainsKey(PlayerPhotonPropertyKey.CurrentRound))
                EmitRoundChangeEvent((int) changedProps[PlayerPhotonPropertyKey.CurrentRound]);
            
            if (changedProps.ContainsKey(PlayerPhotonPropertyKey.InvaderKills))
                EmitInvaderKillsChangeEvent((int) changedProps[PlayerPhotonPropertyKey.InvaderKills]);
        }

        public void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged) { }

        public void OnPlayerEnteredRoom(Player newPlayer) { }

        public void OnPlayerLeftRoom(Player otherPlayer) { }

        public void OnMasterClientSwitched(Player newMasterClient) { }

        #endregion

        public override void InitializeStats() => _photonPlayer.InitializeStats();

        public override void AddScore(int amount) => _photonPlayer.AddScore(amount);

        public override void GoToNextRound() => _photonPlayer.GoToNextRound();

        public override void AddLife() => _photonPlayer.AddLife();

        public override void RemoveLife() => _photonPlayer.RemoveLife();
    }
}