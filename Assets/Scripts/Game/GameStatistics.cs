using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;

namespace SIVS
{
    public class PlayerStatistics
    {
        public int Lives = 3;
        public uint Points = 0;
        public uint InvaderKills = 0;
    }
    
    public class GameStatistics : MonoBehaviourPunCallbacks
    {
        private Dictionary<string, PlayerStatistics> _statistics;

        private string _opponentName; // Cache the opponent name

        public uint TotalInvaderKills
        {
            get
            {
                uint kills = 0;
                foreach (var entry in _statistics)
                    kills += entry.Value.InvaderKills;
                return kills;
            }
        }
        
        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _statistics = new Dictionary<string, PlayerStatistics>();
            foreach (var entry in PhotonNetwork.CurrentRoom.Players)
            {
                _statistics[entry.Value.NickName] = new PlayerStatistics();
                if (entry.Value.NickName != PhotonNetwork.LocalPlayer.NickName)
                    _opponentName = entry.Value.NickName;
            }
                
        }
        
        #endregion

        #region MonoBehaviourPunCallbacks Callbacks
        
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);
            _statistics[newPlayer.NickName] = new PlayerStatistics();
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            base.OnPlayerLeftRoom(otherPlayer);
            _statistics.Remove(otherPlayer.NickName);
        }
        
        #endregion

        public PlayerStatistics GetOwnStatistics()
        {
            return _statistics[PhotonNetwork.LocalPlayer.NickName];
        }

        public PlayerStatistics GetOpponentStatistics()
        {
            return _statistics[_opponentName];
        }

        public PlayerStatistics GetStatistics(string nickName)
        {
            if (!_statistics.ContainsKey(nickName))
                return new PlayerStatistics();
            return _statistics[nickName];
        }
    }
}
