using ExitGames.Client.Photon;
using Photon.Pun;

namespace SIVS
{
    public static class Match
    {
        public static bool IsActive
        {
            set
            {
                PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable()
                {
                    {"Active", value}
                });
            }
            get
            {
                if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("Active", out var isActive))
                    return (bool) isActive;

                return false;
            }
        }
    }
}