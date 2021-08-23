using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine;
using TMPro;

namespace SIVS
{
    public class PlayerInfoUI : MonoBehaviourPunCallbacks
    {
        public Canvas uiCanvas;
        public TMP_Text nameLabel;
        public TMP_Text scoreLabel;
        public TMP_Text roundLabel;
        public GameObject lifeObject;

        private int _actorNumber = -1;

        private readonly List<GameObject> _lifeObjects = new List<GameObject>();

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if (targetPlayer.ActorNumber != _actorNumber)
                return;
            
            if (changedProps.ContainsKey(PlayerStats.Lives))
                UpdateLifeObjects((int) changedProps[PlayerStats.Lives]);
            
            if (changedProps.ContainsKey(PlayerStats.CurrentRound))
                UpdateRound((int) changedProps[PlayerStats.CurrentRound]);
            
            UpdateScore(targetPlayer.GetScore());
        }

        public void Initialize(Player player)
        {
            _actorNumber = player.ActorNumber;
            nameLabel.text = player.NickName;
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

        private void UpdateRound(int round) =>
            roundLabel.text = round > Match.FinalRound ? "Victory!" : $"Round {round}";

        private void UpdateScore(int score) =>
            scoreLabel.text = score.ToString("D5");
    }
}