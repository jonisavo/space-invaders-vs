using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.UtilityScripts;

namespace SIVS
{
    public class PlayerStats
    {
        private const int InitialLives = 3;
        private const int MaxLives = 5;

        public const string Ready = "PlayerReady";
        public const string CurrentRound = "PlayerCurrentRound";
        public const string Lives = "PlayerLives";
        public const string InvaderKills = "PlayerInvaderKills";

        public static void InitializeStats()
        {
            PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable()
            {
                {Ready, true},
                {CurrentRound, 1},
                {Lives, InitialLives},
                {InvaderKills, 0}
            });
            PhotonNetwork.LocalPlayer.SetScore(0);
        }

        public static int GetRound()
        {
            if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(CurrentRound, out var round))
                return (int) round;
            
            return 1;
        }

        public static void NextRound()
        {
            PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable()
            {
                {CurrentRound, GetRound() + 1}
            });
        }

        public static void RemoveLife()
        {
            if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(Lives, out var lives))
                lives = (int) lives - 1;
            else
                lives = InitialLives - 1;

            PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable()
            {
                {Lives, lives}
            });
        }

        public static void AddLife()
        {
            var lives = (int)PhotonNetwork.LocalPlayer.CustomProperties[Lives];
            if (lives >= MaxLives) return;

            PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable()
            {
                {Lives, lives + 1}
            });
        }

        public static void AddScore(int amount) => PhotonNetwork.LocalPlayer.AddScore(amount);

        public static int GetScore() => PhotonNetwork.LocalPlayer.GetScore();

        public static void AddKill()
        {
            if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(InvaderKills, out var kills))
                kills = (int) kills + 1;
            else
                kills = 1;

            PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable()
            {
                {InvaderKills, kills}
            });
        }
    }
}
