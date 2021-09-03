using System.Collections.Generic;
using RedBlueGames.NotNull;
using RedBlueGames.Tools.TextTyper;
using UnityEngine;

namespace SIVS
{
    public class PlayerInfoUI : MonoBehaviour
    {
        [NotNull]
        public Canvas uiCanvas;
        
        [NotNull]
        public TextTyper nameTextTyper;
        
        [NotNull]
        public TextTyper scoreTextTyper;
        
        [Header("Lives")]
        [NotNull]
        public GameObject lifeObject;

        [NotNull]
        public GameObject playerHitPopupObject;

        [NotNull]
        public GameObject playerLostLivesPopupObject;

        [Header("Rounds")]
        [NotNull]
        public TextTyper roundTextTyper;
        
        [NotNull]
        public GameObject nextRoundPopupObject;
        
        [NotNull]
        public RainbowAnimationText roundRainbowTextComponent;

        private int _playerNumber = -1;

        private readonly List<GameObject> _lifeObjects = new List<GameObject>();

        private void Awake() => roundRainbowTextComponent.enabled = false;

        private void OnEnable()
        {
            SIVSPlayer.OnLivesChange += HandleLivesChange;
            SIVSPlayer.OnRoundChange += HandleRoundChange;
            SIVSPlayer.OnScoreChange += HandleScoreChange;
            PlayerHealth.OnHit += HandlePlayerHit;
            GameManager.OnGameEnd += HandleGameEnd;
        }

        private void OnDisable()
        {
            SIVSPlayer.OnLivesChange -= HandleLivesChange;
            SIVSPlayer.OnRoundChange -= HandleRoundChange;
            SIVSPlayer.OnScoreChange -= HandleScoreChange;
            PlayerHealth.OnHit -= HandlePlayerHit;
            GameManager.OnGameEnd -= HandleGameEnd;
        }

        private void HandleGameEnd(SIVSPlayer winner, SIVSPlayer loser, VictoryReason reason)
        {
            if (reason != VictoryReason.Leave || winner.Number == _playerNumber)
                return;
            
            var roundPopupObject = Instantiate(nextRoundPopupObject, gameObject.transform, false);

            var textPopup = roundPopupObject.GetComponent<TextPopup>();
            
            textPopup.ChangeText($"{loser.Name} left.");

            textPopup.Show();
        }

        private void HandleLivesChange(SIVSPlayer player, int newLives)
        {
            if (player.Number != _playerNumber)
                return;
            
            UpdateLifeObjects(newLives);
            
            if (newLives == 0)
                ShowPlayerLostLivesPopup();
        }

        private void HandleRoundChange(SIVSPlayer player, int newRound)
        {
            if (player.Number != _playerNumber)
                return;
            
            UpdateRound(newRound);
            ShowRoundChangePopup(newRound);
        }

        private void HandleScoreChange(SIVSPlayer player, int newScore)
        {
            if (player.Number != _playerNumber)
                return;
            
            UpdateScore(newScore);
        }

        private void HandlePlayerHit(SIVSPlayer player)
        {
            if (player.Number != _playerNumber)
                return;
            
            ShowPlayerHitPopup(player);
        }

        public void Initialize(SIVSPlayer player)
        {
            _playerNumber = player.Number;
            nameTextTyper.TypeText(player.Name);
        }

        private void AddLife()
        {
            var position = new Vector2(_lifeObjects.Count * -0.5f, 0.05f);

            var lifeObj = Instantiate(lifeObject, position, Quaternion.identity);
            
            lifeObj.transform.SetParent(uiCanvas.transform, false);

            _lifeObjects.Add(lifeObj);
        }

        private void RemoveLife()
        {
            if (_lifeObjects.Count == 0)
                return;

            Destroy(_lifeObjects[_lifeObjects.Count - 1]);
            
            _lifeObjects.RemoveAt(_lifeObjects.Count - 1);
        }

        private void UpdateLifeObjects(int newLives)
        {
            while (_lifeObjects.Count != newLives)
            {
                if (_lifeObjects.Count > newLives)
                    RemoveLife();
                else
                    AddLife();
            }
        }

        private void ShowRoundChangePopup(int round)
        {
            if (round == 1 || round > Match.FinalRound)
                return;
            
            var roundPopupObject = Instantiate(nextRoundPopupObject, gameObject.transform, false);

            var textPopup = roundPopupObject.GetComponent<TextPopup>();
            
            if (round == Match.FinalRound)
            {
                textPopup.ChangeText("FINAL ROUND!");
                var rainbowText = textPopup.gameObject.AddComponent<RainbowAnimationText>();
                rainbowText.EnableAllAnimation();
                rainbowText.animationSpeed = 25;
            }

            textPopup.Show();
        }

        private void ShowPlayerHitPopup(SIVSPlayer player)
        {
            if (player.Lives <= 1)
                return;

            Instantiate(playerHitPopupObject, gameObject.transform, false);
        }

        private void ShowPlayerLostLivesPopup()
        {
            Instantiate(playerLostLivesPopupObject, gameObject.transform.position, Quaternion.identity);
        }

        private void UpdateRound(int round)
        {
            if (round >= Match.FinalRound)
                roundRainbowTextComponent.enabled = true;
            
            roundTextTyper.TypeText(round > Match.FinalRound ? "Victory!" : $"Round {round}", 0.05f);
        }

        private void UpdateScore(int score) =>
            scoreTextTyper.TypeText(score.ToString("D5"));
    }
}