using RedBlueGames.NotNull;
using UnityEngine;
using UnityEngine.UI;

namespace SIVS
{
    [RequireComponent(typeof(ScrollRect))]
    public class ScrollRectArrows : MonoBehaviour
    {
        [NotNull]
        public Image upArrowImage;

        [NotNull]
        public Image downArrowImage;

        private ScrollRect _scrollRect;
        
        private void Awake()
        {
            _scrollRect = GetComponent<ScrollRect>();
            
            _scrollRect.onValueChanged.AddListener(HandleScroll);
            
            HandleScroll(Vector2.one);
        }

        private void HandleScroll(Vector2 newLocation)
        {
            upArrowImage.enabled = newLocation.y < 0.99f;

            downArrowImage.enabled = newLocation.y > 0.01f;
        }
    }
}