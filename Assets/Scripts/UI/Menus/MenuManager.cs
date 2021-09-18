using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SIVS
{
    public class MenuManager : MonoBehaviour
    {
        [Min(0)]
        public int id;
        
        public SerializableDictionary<string, Menu> menus;

        public string defaultMenu;

        private EventSystem _eventSystem;

        private readonly Stack<Menu> _history = new Stack<Menu>();

        private delegate void PushMenuDelegate(string menuName, int managerId = 0);

        private static event PushMenuDelegate OnPushMenu;

        public static bool BlockGoingBack;

        public static void PushMenu(string menuName, int managerId = 0) =>
            OnPushMenu?.Invoke(menuName, managerId);

        private void OnEnable()
        {
            OnPushMenu += HandlePushMenu;
        }

        private void OnDisable()
        {
            OnPushMenu -= HandlePushMenu;
        }

        private void Awake()
        {
            _eventSystem = EventSystem.current;
            
            if (!string.IsNullOrEmpty(defaultMenu))
                Push(defaultMenu);
        }

        private void Update()
        {
            if (Input.GetButtonDown("Cancel"))
                GoBack();
            else if (!_eventSystem.currentSelectedGameObject)
                AttemptRestoringSelection();
        }

        private void AttemptRestoringSelection()
        {
            if (_history.Count == 0 || (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0))
                return;

            var automaticSelection = _history.Peek().GetAutomaticSelection();
            
            if (automaticSelection)
                automaticSelection.Select();
        }

        public void Push(string menuName)
        {
            if (!menus.TryGetValue(menuName, out var menuToPush) || menuToPush == null)
            {
                Debug.LogError("No menu named " + menuName + " found in MenuManager", gameObject);
                return;
            }

            if (_history.Count > 0)
                if (!menuToPush.showOnTop)
                    _history.Peek().Hide();
                else
                    _history.Peek().MakeNonInteractable();

            _history.Push(menuToPush);
            
            _history.Peek().Show();
        }

        public void Pop()
        {
            if (_history.Count == 0)
                return;

            _history.Pop().Hide();

            _history.Peek().Show();
        }

        public void GoBack()
        {
            if (BlockGoingBack || _history.Count <= 1 || _history.Peek().disableGoingBack)
                return;

            Pop();
        }
        
        private void HandlePushMenu(string menuName, int managerId = 0)
        {
            if (managerId != id)
                return;
            
            Push(menuName);
        }

        public void ToggleCurrentMenuInteractivity(bool value)
        {
            if (_history.Count == 0)
                return;
            
            if (value)
                _history.Peek().MakeInteractable();
            else
                _history.Peek().MakeNonInteractable();
        }
    }
}
