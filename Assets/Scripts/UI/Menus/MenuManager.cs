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

        [Header("Set default menu interactable if requirements are met:")]
        [Tooltip("If set, sets the default menu as interactable after the bar canvas has opened.")]
        public BarCanvas barCanvasInteractableRequirement;

        private EventSystem _eventSystem;

        private readonly Stack<Menu> _history = new Stack<Menu>();

        private delegate void PushMenuDelegate(string menuName, int managerId = 0);

        private static event PushMenuDelegate OnPushMenu;

        public static void PushMenu(string menuName, int managerId = 0) =>
            OnPushMenu?.Invoke(menuName, managerId);

        private void OnEnable()
        {
            OnPushMenu += HandlePushMenu;

            if (barCanvasInteractableRequirement)
                barCanvasInteractableRequirement.OnFinishOpen += HandleBarCanvasOpen;
        }

        private void OnDisable()
        {
            OnPushMenu -= HandlePushMenu;

            barCanvasInteractableRequirement.OnFinishOpen -= HandleBarCanvasOpen;
        }

        private void Awake()
        {
            _eventSystem = EventSystem.current;
            
            if (!string.IsNullOrEmpty(defaultMenu))
                Push(defaultMenu, barCanvasInteractableRequirement == null);
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
            if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
                return;
            
            var currentMenu = _history.Peek();
            
            if (currentMenu.autoSelect)
                currentMenu.autoSelect.Select();
        }

        public void Push(string menuName) => Push(menuName, true);

        public void Push(string menuName, bool makeInteractable)
        {
            if (!menus.ContainsKey(menuName))
            {
                Debug.LogError("No menu named " + menuName + " found in MenuManager", gameObject);
                return;
            }
            
            if (_history.Count > 0)
                _history.Peek().Hide();
            
            _history.Push(menus[menuName]);
            
            _history.Peek().Show(makeInteractable);
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
            if (_history.Count <= 1 || _history.Peek().disableGoingBack)
                return;

            Pop();
        }

        private void HandlePushMenu(string menuName, int managerId = 0)
        {
            if (managerId != id)
                return;
            
            Push(menuName);
        }

        private void HandleBarCanvasOpen()
        {
            var currentMenu = _history.Peek();
            
            currentMenu.MakeInteractable();
            currentMenu.SelectPrimaryElement();
        }
    }
}
