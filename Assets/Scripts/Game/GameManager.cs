using System.Collections.Generic;
using UnityEngine;

namespace SIVS
{
    [RequireComponent(typeof(VictoryUIManager))]
    [RequireComponent(typeof(InvaderManager))]
    [RequireComponent(typeof(OptionsManager))]
    public class GameManager : MonoBehaviour
    {
        [Tooltip("Audio clip to play upon a victory.")]
        public AudioClip victorySound;

        public static readonly Dictionary<int, SIVSPlayer> Players =
            new Dictionary<int, SIVSPlayer>();

        protected bool _gameOver;

        private bool _bothReady;

        private InvaderManager _invaderManager;

        private VictoryUIManager _victoryUIManager;

        private OptionsManager _optionsManager;

        protected virtual void Awake()
        {
            CleanupPlayers();
            
            _invaderManager = GetComponent<InvaderManager>();
            _victoryUIManager = GetComponent<VictoryUIManager>();
            _optionsManager = GetComponent<OptionsManager>();
            
            InitializePlayers();
        }

        protected virtual void OnEnable()
        {
            SIVSPlayer.OnReadyChange += HandleReadyChange;
            SIVSPlayer.OnLivesChange += HandleLivesChange;
            SIVSPlayer.OnRoundChange += HandleRoundChange;
        }

        protected virtual void OnDisable()
        {
            SIVSPlayer.OnReadyChange -= HandleReadyChange;
            SIVSPlayer.OnLivesChange -= HandleLivesChange;
            SIVSPlayer.OnRoundChange -= HandleRoundChange;
        }

        private void HandleReadyChange(SIVSPlayer player, bool isReady)
        {
            if (_bothReady || _gameOver || !IsEveryoneReady())
                return;

            _invaderManager.InitializeAllInvaders();
            
            _bothReady = true;
            
            Match.IsActive = true;
        }

        private void HandleLivesChange(SIVSPlayer player, int newLives)
        {
            if (newLives > 0 || _gameOver)
                return;

            var otherPlayerNumber = player.Number == 1 ? 2 : 1;
            
            EndGame(Players[otherPlayerNumber],
                Players[player.Number],
                VictoryReason.LastStanding
            );
        }

        private void HandleRoundChange(SIVSPlayer player, int newRound)
        {
            if (newRound <= Match.FinalRound || _gameOver)
                return;
            
            var otherPlayerNumber = player.Number == 1 ? 2 : 1;
            
            EndGame(Players[player.Number],
                Players[otherPlayerNumber],
                VictoryReason.Round5
            );
        }

        protected virtual void InitializePlayers()
        {
            Players[1] = new SIVSPlayer("Player 1", 1);
            Players[2] = new SIVSPlayer("Player 2", 2);

            Players[1].InitializeStats();
            Players[2].InitializeStats();
        }

        protected void CleanupPlayers()
        {
            foreach (var player in Players.Values)
                player.Cleanup();
            
            Players.Clear();
        }

        protected virtual void EndGame(SIVSPlayer winner, SIVSPlayer loser, VictoryReason victoryReason)
        {
            _gameOver = true;
            
            Match.IsActive = false;

            _optionsManager.CloseCanvas();

            ShowVictoryScreen(winner, loser, victoryReason);

            GameObject.Find("Music Player")
                .GetComponent<AudioSource>()
                .Stop();
            
            if (victorySound)
                SoundPlayer.PlaySound(victorySound);

            StopGameProcessing();
        }
        
        private void ShowVictoryScreen(SIVSPlayer winner, SIVSPlayer loser, VictoryReason victoryReason)
        {
            var winnerNickName = winner == null ? "No one" : winner.Name;

            var loserNickName = loser == null ? "The opponent" : loser.Name;

            _victoryUIManager.ShowVictoryScreen(winnerNickName, loserNickName, victoryReason);
        }

        protected void StopGameProcessing()
        {
            StopAllCoroutines();
            DestroyBullets();
            DestroyPowerups();
            _invaderManager.StopAllCoroutines();

            foreach (var invader in GameObject.FindGameObjectsWithTag("Invader"))
                invader.GetComponent<InvaderShoot>().StopShooting();
        }

        protected void DestroyBullets()
        {
            foreach (var playerBullet in GameObject.FindGameObjectsWithTag("PlayerBullet"))
                Destroy(playerBullet);

            foreach (var enemyBullet in GameObject.FindGameObjectsWithTag("EnemyBullet"))
                Destroy(enemyBullet);
        }

        protected virtual void DestroyPowerups()
        {
            foreach (var powerUp in GameObject.FindGameObjectsWithTag("Powerup"))
                Destroy(powerUp);
        }
        
        private bool IsEveryoneReady()
        {
            foreach (var player in Players.Values)
            {
                if (player == null)
                    return false;

                if (!player.Ready)
                    return false;
            }

            return true;
        }
    }
}