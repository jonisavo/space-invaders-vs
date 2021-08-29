using ExitGames.Client.Photon;
using Photon.Realtime;
using UnityEngine;

namespace SIVS
{
    public static class PhotonRealtimePlayerSIVSExtensions
    {
        public static void InitializeStats(this Player player)
        {
            if (!player.IsLocal)
                return;
            
            player.SetCustomProperties(new Hashtable()
            {
                { PlayerPhotonPropertyKey.Ready, false },
                { PlayerPhotonPropertyKey.Score, 0 },
                { PlayerPhotonPropertyKey.CurrentRound, 1 },
                { PlayerPhotonPropertyKey.Lives, SIVSPlayer.InitialLives },
                { PlayerPhotonPropertyKey.InvaderKills, 0 },
                { PlayerPhotonPropertyKey.BulletType, PlayerBulletType.Normal }
            });
        }

        private static T GetProperty<T>(this Player player, string key, T defaultValue)
        {
            if (player.CustomProperties.TryGetValue(key, out var value))
                return (T) value;
            
            return defaultValue;
        }

        private static void SetProperty(this Player player, string key, object value)
        {
            if (!player.IsLocal)
                return;
            
            player.SetCustomProperties(new Hashtable()
            {
                { key, value }
            });
        }

        public static void SetReady(this Player player, bool value) =>
            player.SetProperty(PlayerPhotonPropertyKey.Ready, value);

        public static bool IsReady(this Player player) =>
            player.GetProperty(PlayerPhotonPropertyKey.Ready, false);

        public static int GetRound(this Player player) =>
            player.GetProperty(PlayerPhotonPropertyKey.CurrentRound, 1);

        public static void SetRound(this Player player, int newRound) =>
            player.SetProperty(PlayerPhotonPropertyKey.CurrentRound, newRound);

        public static int GetScore(this Player player) =>
            player.GetProperty(PlayerPhotonPropertyKey.Score, 0);

        public static void AddScore(this Player player, int amount) =>
            player.SetProperty(PlayerPhotonPropertyKey.Score, player.GetScore() + amount);

        public static int GetInvaderKills(this Player player) =>
            player.GetProperty(PlayerPhotonPropertyKey.InvaderKills, 0);

        public static void SetInvaderKills(this Player player, int newKills) =>
            player.SetProperty(PlayerPhotonPropertyKey.InvaderKills, newKills);

        public static void GoToNextRound(this Player player) =>
            player.SetRound(player.GetRound() + 1);

        public static int GetLives(this Player player) =>
            player.GetProperty(PlayerPhotonPropertyKey.Lives, SIVSPlayer.InitialLives);

        public static void SetLives(this Player player, int newLives) =>
            player.SetProperty(PlayerPhotonPropertyKey.Lives, newLives);

        public static void RemoveLife(this Player player)
        {
            var newLives = Mathf.Clamp(player.GetLives() - 1, 0, SIVSPlayer.MaxLives);

            player.SetLives(newLives);
        }

        public static void AddLife(this Player player)
        {
            var newLives = Mathf.Clamp(player.GetLives() + 1, 0, SIVSPlayer.MaxLives);

            player.SetLives(newLives);
        }

        public static PlayerBulletType GetBulletType(this Player player) =>
            player.GetProperty(PlayerPhotonPropertyKey.BulletType, PlayerBulletType.Normal);

        public static void SetBulletType(this Player player, PlayerBulletType type) =>
            player.SetProperty(PlayerPhotonPropertyKey.BulletType, type);
    }
}