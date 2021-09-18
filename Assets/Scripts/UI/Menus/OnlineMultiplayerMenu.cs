using RedBlueGames.NotNull;
using UnityEngine;
using UnityEngine.UI;

namespace SIVS
{
    public class OnlineMultiplayerMenu : Menu
    {
        [Header("Online Multiplayer Menu")]
        [NotNull]
        [Tooltip("The CanvasGroup to show when the client is still connecting.")]
        public CanvasGroup connectingCanvasGroup;
        
        [NotNull]
        [Tooltip("The CanvasGroup to show when the client is connected.")]
        public CanvasGroup menuCanvasGroup;

        [NotNull]
        [Tooltip("The Selectable to automatically select in the menu Canvas Group if it is open.")]
        public Selectable menuAutoSelect;

        private bool _applicationIsQuitting;

        private bool _connectedOnce;

        protected override void OnEnable()
        {
            base.OnEnable();
            
            NetworkManager.OnConnect += SwitchToMenuPage;
            NetworkManager.OnDisconnect += SwitchToConnectingPage;
            Application.quitting += HandleApplicationQuitting;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            NetworkManager.OnConnect -= SwitchToMenuPage;
            NetworkManager.OnDisconnect -= SwitchToConnectingPage;
            Application.quitting -= HandleApplicationQuitting;
        }

        public override void Show()
        {
            if (_applicationIsQuitting)
                return;
            
            if (!_connectedOnce)
                _connectedOnce = NetworkManager.ConnectUsingCurrentManager();

            if (NetworkManager.IsConnectedUsingCurrentManager())
                SwitchToMenuPage();
            else
                SwitchToConnectingPage();

            base.Show();
        }

        public override Selectable GetAutomaticSelection() =>
            menuAutoSelect.IsInteractable() ? menuAutoSelect : base.GetAutomaticSelection();

        public override void MakeInteractable()
        {
            base.MakeInteractable();

            var childCanvasGroup = NetworkManager.IsConnectedUsingCurrentManager()
                ? menuCanvasGroup
                : connectingCanvasGroup;
            
            MakeCanvasGroupInteractable(childCanvasGroup);
        }

        private void HandleApplicationQuitting() => _applicationIsQuitting = true;

        private void SwitchToMenuPage()
        {
            HideCanvasGroup(connectingCanvasGroup);
            
            ShowCanvasGroup(menuCanvasGroup);
        }

        private void SwitchToConnectingPage()
        {
            HideCanvasGroup(menuCanvasGroup);
            
            ShowCanvasGroup(connectingCanvasGroup);
        }
    }
}
