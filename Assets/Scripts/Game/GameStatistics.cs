using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace SIVS
{
    public class PlayerStatistics
    {
        public uint Points = 0;
        public uint InvaderKills = 0;
        public uint FiredBullets = 0;
    }
    
    public class GameStatistics : MonoBehaviour
    {
        private Dictionary<int, PlayerStatistics> _statistics;

        private void Awake()
        {
            _statistics = new Dictionary<int, PlayerStatistics>();
            foreach (var entry in PhotonNetwork.CurrentRoom.Players)
                _statistics[entry.Key] = new PlayerStatistics();
        }

        public PlayerStatistics GetOwnStatistics()
        {
            return _statistics[PhotonNetwork.LocalPlayer.ActorNumber];
        }

        public PlayerStatistics GetOpponentStatistics()
        {
            foreach (var entry in PhotonNetwork.CurrentRoom.Players)
                if (entry.Value.ActorNumber != PhotonNetwork.LocalPlayer.ActorNumber)
                    return _statistics[entry.Key];
            return new PlayerStatistics();
        }

        public uint TotalInvaderKills()
        {
            uint kills = 0;
            foreach (var entry in _statistics)
                kills += entry.Value.InvaderKills;
            return kills;
        }
    }
}
