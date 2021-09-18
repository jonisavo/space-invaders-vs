using System.Collections.Generic;
using RedBlueGames.NotNull;
using UnityEngine;

namespace SIVS
{
    [RequireComponent(typeof(MenuManager))]
    public class MenuManagerBarCanvasHandler : MonoBehaviour
    {
        [NotNull (IgnorePrefab = true)]
        [Tooltip("The bar canvas to react to.")]
        public BarCanvas barCanvas;

        private MenuManager _menuManager;

        private readonly Stack<bool> _moveStatuses = new Stack<bool>();

        private void Awake() => _menuManager = GetComponent<MenuManager>();

        private void OnEnable()
        {
            barCanvas.OnStartOpen += HandleBarMovementStart;
            barCanvas.OnStartClose += HandleBarMovementStart;
            barCanvas.OnFinishOpen += HandleBarMovementEnd;
            barCanvas.OnFinishClose += HandleBarMovementEnd;
        }

        private void OnDisable()
        {
            barCanvas.OnStartOpen -= HandleBarMovementStart;
            barCanvas.OnStartClose -= HandleBarMovementStart;
            barCanvas.OnFinishOpen -= HandleBarMovementEnd;
            barCanvas.OnFinishClose -= HandleBarMovementEnd;
        }

        private void HandleBarMovementStart()
        {
            _moveStatuses.Push(true);
            _menuManager.ToggleCurrentMenuInteractivity(false);
            MenuManager.BlockGoingBack = true;
        }

        private void HandleBarMovementEnd()
        {
            _moveStatuses.Pop();

            var areBarsMoving = _moveStatuses.Count > 0 && _moveStatuses.Peek();

            _menuManager.ToggleCurrentMenuInteractivity(!areBarsMoving);

            MenuManager.BlockGoingBack = areBarsMoving;
        }
    }
}