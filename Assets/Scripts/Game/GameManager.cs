using System;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Random = UnityEngine.Random;

namespace SIVS
{
    [RequireComponent(typeof(VictoryUIManager))]
    [RequireComponent(typeof(InvaderManager))]
    [RequireComponent(typeof(OptionsManager))]
    public class GameManager : MonoBehaviourPunCallbacks
    {
        [Tooltip("Audio clip to play upon a victory.")]
        public AudioClip victorySound;

        private bool _bothReady;

        private bool _gameOver;

        private InvaderManager _invaderManager;

        public delegate void RoundChangeDelegate(Player player, int round);

        public static event RoundChangeDelegate OnRoundChange;

        #region Unity Callbacks

        private void Awake()
        {
            PlayerStats.InitializeStats(PhotonNetwork.LocalPlayer);
            Random.InitState((int) DateTime.Now.Ticks + PhotonNetwork.LocalPlayer.ActorNumber);
            _invaderManager = GetComponent<InvaderManager>();
        }

        private void Start()
        {
            PlayerStats.SetReady(PhotonNetwork.LocalPlayer, true);
        }

        #endregion

        #region PUN Callbacks

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if (_gameOver) return;

            if (changedProps.ContainsKey(PlayerStats.Ready))
            {
                if (!_bothReady && IsEveryoneReady())
                {
                    _bothReady = true;

                    _invaderManager.InitializeInvaders();

                    if (PhotonNetwork.IsMasterClient)
                        Match.IsActive = true;
                }
            }

            if (changedProps.ContainsKey(PlayerStats.Lives))
            {
                if ((int) changedProps[PlayerStats.Lives] <= 0)
                    EndGame(GetOtherPlayer(targetPlayer), targetPlayer, VictoryReason.LastStanding);
            }

            if (!changedProps.ContainsKey(PlayerStats.CurrentRound))
                return;

            var round = (int) changedProps[PlayerStats.CurrentRound];
            
            OnRoundChange?.Invoke(targetPlayer, round);

            if (round > Match.FinalRound) 
                EndGame(targetPlayer, GetOtherPlayer(targetPlayer), VictoryReason.Round5);
        }

        public override void OnLeftRoom()
        {
            SceneManager.LoadScene("MainMenu");
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            if (_gameOver) return;

            EndGame(GetOtherPlayer(otherPlayer), otherPlayer, VictoryReason.Leave);
        }

        #endregion

        public void LeaveGame() => PhotonNetwork.LeaveRoom();

        private bool IsEveryoneReady()
        {
            foreach (var player in PhotonNetwork.CurrentRoom.Players.Values)
            {
                if (player == null) return false;

                if (!player.CustomProperties.ContainsKey(PlayerStats.Ready))
                    return false;

                if (!(bool)player.CustomProperties[PlayerStats.Ready])
                    return false;
            }

            return true;
        }

        private Player GetOtherPlayer(Player firstPlayer)
        {
            foreach (var player in PhotonNetwork.CurrentRoom.Players.Values)
                if (player.ActorNumber != firstPlayer.ActorNumber)
                    return player;

            return null;
        }

        private void EndGame(Player winner, Player loser, VictoryReason victoryReason)
        {
            _gameOver = true;

            if (PhotonNetwork.IsMasterClient)
                Match.IsActive = false;

            PlayerStats.SetReady(PhotonNetwork.LocalPlayer, false);

            GetComponent<OptionsManager>().CloseCanvas();

            ShowVictoryScreen(winner, loser, victoryReason);

            GameObject.Find("Music Player")
                .GetComponent<AudioSource>()
                .Stop();

            if (winner != null && winner.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
                SoundPlayer.PlaySound(victorySound);

            SetHighScore();

            StopGameProcessing();
        }

        private void ShowVictoryScreen(Player winner, Player loser, VictoryReason victoryReason)
        {
            var winnerNickName = winner == null ? "No one" : winner.NickName;

            var loserNickName = loser == null ? "The opponent" : loser.NickName;

            GetComponent<VictoryUIManager>()
                .ShowVictoryScreen(winnerNickName, loserNickName, victoryReason);
        }

        private void StopGameProcessing()
        {
            StopAllCoroutines();
            DestroyBullets();
            DestroyPowerups();
            _invaderManager.StopAllCoroutines();

            foreach (var invader in GameObject.FindGameObjectsWithTag("Invader"))
                invader.GetComponent<InvaderShoot>().StopShooting();
        }

        private void SetHighScore()
        {
            var highScore = PlayerPrefs.GetInt("HighScore", 0);

            var score = PhotonNetwork.LocalPlayer.GetScore();

            if (score > highScore)
                PlayerPrefs.SetInt("HighScore", score);
        }

        private void DestroyBullets()
        {
            foreach (var playerBullet in GameObject.FindGameObjectsWithTag("PlayerBullet"))
                Destroy(playerBullet);

            foreach (var enemyBullet in GameObject.FindGameObjectsWithTag("EnemyBullet"))
                Destroy(enemyBullet);
        }

        private void DestroyPowerups()
        {
            foreach (var powerUp in GameObject.FindGameObjectsWithTag("Powerup"))
            {
                var powerUpPhotonView = powerUp.GetPhotonView();

                if (!powerUpPhotonView.IsMine) continue;

                powerUpPhotonView.RPC("DestroyPowerup", RpcTarget.All);
            }
        }
    }
}
