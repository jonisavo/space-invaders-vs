using System.Collections;
using RedBlueGames.NotNull;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SIVS
{
    [RequireComponent(typeof(Selectable))]
    [RequireComponent(typeof(RectTransform))]
    public class ScrollRectItem : MonoBehaviour, ISelectHandler
    {
        [NotNull]
        public ScrollRect parentScrollRect;

        private RectTransform _rectTransform;

        private void Awake() => _rectTransform = GetComponent<RectTransform>();

        private void OnEnable() => StartCoroutine(ScrollWhenReady());

        private IEnumerator ScrollWhenReady()
        {
            yield return new WaitForEndOfFrame();

            var toggle = GetComponent<Selectable>() as Toggle;
            
            if (toggle && toggle.isOn)
                ScrollToThisItem();
        }

        public void OnSelect(BaseEventData eventData)
        {
            ScrollToThisItem();
        }

        private void ScrollToThisItem()
        {
            var currentAnchoredY = _rectTransform.anchoredPosition.y;
            var verticalSizeHalf = _rectTransform.sizeDelta.y / 2;
            var scrollRectHeight = parentScrollRect.content.sizeDelta.y;
            
            var posWithBottomPivot =
                (currentAnchoredY - verticalSizeHalf) / scrollRectHeight;

            if (posWithBottomPivot > 0.5f)
                parentScrollRect.verticalNormalizedPosition =
                    (currentAnchoredY + verticalSizeHalf) / scrollRectHeight;
            else
                parentScrollRect.verticalNormalizedPosition = posWithBottomPivot;
        }
    }
}