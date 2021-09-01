using System.Collections.Generic;
using UnityEngine;

namespace SIVS
{
    public class MenuManager : MonoBehaviour
    {
        [Min(0)]
        public int id;
        
        public SerializableDictionary<string, Menu> menus;

        public string defaultMenu;

        private readonly Stack<Menu> _history = new Stack<Menu>();

        private delegate void PushMenuDelegate(string menuName, int managerId = 0);

        private static event PushMenuDelegate OnPushMenu;

        public static void PushMenu(string menuName, int managerId = 0) =>
            OnPushMenu?.Invoke(menuName, managerId);

        private void OnEnable() => OnPushMenu += HandlePushMenu;

        private void OnDisable() => OnPushMenu -= HandlePushMenu;
        
        private void Awake()
        {
            if (!string.IsNullOrEmpty(defaultMenu))
                Push(defaultMenu);
        }

        private void Update()
        {
            if (Input.GetButtonDown("Cancel"))
                GoBack();
        }

        public void Push(string menuName)
        {
            if (!menus.ContainsKey(menuName))
            {
                Debug.LogError("No menu named " + menuName + " found in MenuManager", gameObject);
                return;
            }
            
            if (_history.Count > 0)
                _history.Peek().Hide();
            
            _history.Push(menus[menuName]);
            
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
    }
}
