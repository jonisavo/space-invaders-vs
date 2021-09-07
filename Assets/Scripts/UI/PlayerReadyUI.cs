﻿using RedBlueGames.NotNull;
using UnityEngine;

namespace SIVS
{
    public class PlayerReadyUI : MonoBehaviour
    {
        [Min(1)]
        public uint playerNumber = 1;

        [NotNull]
        public GameObject readyPromptObject;

        [NotNull]
        public GameObject readyConfirmationObject;

        public bool Ready
        {
            get => _ready;
            private set
            {
                _ready = value;
                OnReadyChange?.Invoke(value, playerNumber);
            }
        }

        private bool _ready;

        public delegate void ReadyChangeDelegate(bool value, uint playerNumber);

        public event ReadyChangeDelegate OnReadyChange;
        
        private string _inputName;

        private void Awake()
        {
            _inputName = $"Player {playerNumber} Fire";
        }

        private void Update()
        {
            if (Ready)
                return;
            
            if (Input.GetButtonDown(_inputName))
                SetReady();
        }

        public void SetReady()
        {
            readyPromptObject.SetActive(false);
            readyConfirmationObject.SetActive(true);
            Ready = true;
        }

        public void SetNotReady()
        {
            readyPromptObject.SetActive(true);
            readyConfirmationObject.SetActive(true);
            Ready = false;
        }
    }
}