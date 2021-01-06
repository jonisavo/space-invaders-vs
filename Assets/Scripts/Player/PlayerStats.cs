using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;

namespace SIVS
{
    public static class PlayerStats
    {
        private const int InitialLives = 3;
        private const int MaxLives = 5;

        public const string Ready = "PlayerReady";
        public const string CurrentRound = "PlayerCurrentRound";
        public const string Lives = "PlayerLives";
        public const string InvaderKills = "PlayerInvaderKills";

        public static void InitializeStats(Player player)
        {
            player.SetCustomProperties(new Hashtable()
            {
                {Ready, true},
                {CurrentRound, 1},
                {Lives, InitialLives},
                {InvaderKills, 0}
            });
            player.SetScore(0);
        }

        public static int GetRound(Player player)
        {
            if (player.CustomProperties.TryGetValue(CurrentRound, out var round))
                return (int) round;
            
            return 1;
        }

        public static int GetOwnRound() => GetRound(PhotonNetwork.LocalPlayer);

        public static void GoToNextRound(Player player)
        {
            player.SetCustomProperties(new Hashtable()
            {
                {CurrentRound, GetRound(player) + 1}
            });
        }

        public static void RemoveLife(Player player)
        {
            if (player.CustomProperties.TryGetValue(Lives, out var lives))
                lives = (int) lives - 1;
            else
                lives = InitialLives - 1;

            player.SetCustomProperties(new Hashtable()
            {
                {Lives, lives}
            });
        }

        public static void AddLife(Player player)
        {
            var lives = (int) player.CustomProperties[Lives];
            if (lives >= MaxLives) return;

            player.SetCustomProperties(new Hashtable()
            {
                {Lives, lives + 1}
            });
        }

        public static bool HasMaximumLives(Player player)
        {
            if (!player.CustomProperties.ContainsKey(Lives))
                return false;

            return (int) player.CustomProperties[Lives] >= MaxLives;
        }
    }
}
