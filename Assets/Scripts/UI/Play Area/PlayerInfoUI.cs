using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using RedBlueGames.Tools.TextTyper;
using UnityEngine;

namespace SIVS
{
    public class PlayerInfoUI : MonoBehaviourPunCallbacks
    {
        public Canvas uiCanvas;
        public TextTyper nameTextTyper;
        public TextTyper scoreTextTyper;
        public GameObject lifeObject;

        [Header("Rounds")]
        public TextTyper roundTextTyper;
        public GameObject nextRoundPopupObject;
        public RainbowGradientText roundRainbowTextComponent;

        private int _actorNumber = -1;

        private readonly List<GameObject> _lifeObjects = new List<GameObject>();

        private int _cachedScore = -1;
        
        private void Awake() => roundRainbowTextComponent.enabled = false;

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if (targetPlayer.ActorNumber != _actorNumber)
                return;
            
            if (changedProps.ContainsKey(PlayerPhotonPropertyKey.Lives))
                UpdateLifeObjects((int) changedProps[PlayerPhotonPropertyKey.Lives]);
            
            if (changedProps.ContainsKey(PlayerPhotonPropertyKey.CurrentRound))
            {
                UpdateRound((int) changedProps[PlayerPhotonPropertyKey.CurrentRound]);
                ShowRoundChangePopup((int) changedProps[PlayerPhotonPropertyKey.CurrentRound]);
            }

            var score = targetPlayer.GetScore();
            
            if (score != _cachedScore)
                UpdateScore(targetPlayer.GetScore());

            _cachedScore = score;
        }

        public void Initialize(Player player)
        {
            _actorNumber = player.ActorNumber;
            nameTextTyper.TypeText(player.NickName);
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
                var rainbowText = textPopup.gameObject.AddComponent<RainbowGradientText>();
                rainbowText.EnableAllAnimation();
                rainbowText.animationSpeed = 25;
            }

            textPopup.Show();
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