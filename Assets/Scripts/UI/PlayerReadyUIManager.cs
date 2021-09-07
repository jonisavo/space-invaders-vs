using System.Collections.Generic;
using UnityEngine;

namespace SIVS
{
    public class PlayerReadyUIManager : MonoBehaviour
    {
        private readonly List<PlayerReadyUI> _readyUIList = new List<PlayerReadyUI>();

        public delegate void AllReadyDelegate();

        public event AllReadyDelegate OnAllReady;
        
        private void Awake() => GetComponentsInChildren(_readyUIList);

        private void OnEnable()
        {
            foreach (var readyUI in _readyUIList)
                readyUI.OnReadyChange += HandleReadyChange;
        }

        private void OnDisable()
        {
            foreach (var readyUI in _readyUIList)
                readyUI.OnReadyChange -= HandleReadyChange;
        }

        private void HandleReadyChange(bool value, uint playerNumber)
        {
            if (IsEveryoneReady())
                OnAllReady?.Invoke();
        }

        private bool IsEveryoneReady()
        {
            foreach (var readyUI in _readyUIList)
                if (!readyUI.Ready)
                    return false;
            
            return true;
        }

        public void SetAllNotReady()
        {
            foreach (var readyUI in _readyUIList)
                readyUI.SetReady(false);
        }
    }
}