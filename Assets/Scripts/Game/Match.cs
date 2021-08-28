using ExitGames.Client.Photon;
using Photon.Pun;

namespace SIVS
{
    public static class Match
    {
        public const string ActivePropertyKey = "Active";

        public const int FinalRound = 5;

        public static bool IsOnline = true;

        public static bool IsActive
        {
            set
            {
                if (IsOnline)
                    SetIsActiveOnline(value);
                else
                    _active = value;
            }
            get => IsOnline ? GetIsActiveOnline() : _active;
        }
        
        private static bool _active;

        private static void SetIsActiveOnline(bool value)
        {
            if (!PhotonNetwork.IsMasterClient)
                return;
                
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable()
            {
                { ActivePropertyKey, value }
            });
        }

        private static bool GetIsActiveOnline()
        {
            if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(ActivePropertyKey, out var isActive))
                return (bool) isActive;

            return false;
        }

        public static int SumOfRounds
        {
            get
            {
                var sum = 0;
                
                foreach (var player in GameManager.Players.Values)
                    sum += player.CurrentRound;
                
                return sum;
            }
        }

        public static SIVSPlayer GetPlayer(int num) => GameManager.Players[num];
    }
}