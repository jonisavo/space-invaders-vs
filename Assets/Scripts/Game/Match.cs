using ExitGames.Client.Photon;
using Photon.Pun;

namespace SIVS
{
    public static class Match
    {
        public const string ActivePropertyKey = "Active";

        public const int FinalRound = 5;

        public static bool IsActive
        {
            set
            {
                PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable()
                {
                    { ActivePropertyKey, value }
                });
            }
            get
            {
                if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(ActivePropertyKey, out var isActive))
                    return (bool) isActive;

                return false;
            }
        }
        
        public static int SumOfRounds
        {
            get
            {
                var sum = 0;
                
                foreach (var player in PhotonNetwork.CurrentRoom.Players.Values)
                    sum += player.GetRound();
                
                return sum;
            }
        }

        public static SIVSPlayer GetPlayer(int num) => GameManager.Players[num];
    }
}