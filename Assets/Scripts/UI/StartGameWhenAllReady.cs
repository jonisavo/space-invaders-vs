using System.Collections;
using RedBlueGames.NotNull;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SIVS
{
    [RequireComponent(typeof(PlayerReadyUIManager))]
    public class StartGameWhenAllReady : MonoBehaviour
    {
        [NotNull]
        public BarCanvas barCanvasToClose;
        
        private PlayerReadyUIManager _playerReadyUIManager;

        private void Awake() => _playerReadyUIManager = GetComponent<PlayerReadyUIManager>();

        private void OnEnable() => _playerReadyUIManager.OnAllReady += HandleAllReady;

        private void OnDisable() => _playerReadyUIManager.OnAllReady -= HandleAllReady;
        
        private void HandleAllReady()
        {
            StartCoroutine(StartGameCoroutine());
        }

        private IEnumerator StartGameCoroutine()
        {
            // TODO: block menu manager automatically when bars are opening/closing?
            MenuManager.BlockGoingBack = true;
            
            yield return barCanvasToClose.CloseBars();

            SceneManager.LoadScene("InGameOffline");

            MenuManager.BlockGoingBack = false;
        }
    }
}
