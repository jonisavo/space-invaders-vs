using System.Collections;
using RedBlueGames.NotNull;
using UnityEngine;

namespace SIVS
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(CanvasGroup))]
    public class BarCanvas : MonoBehaviour
    {
        [Header("Bars")]
        [NotNull]
        public RectTransform topBarTransform;

        [NotNull]
        public RectTransform bottomBarTransform;

        public AnimationCurve barMovementCurve = AnimationCurve.Constant(0f, 1f, 1f);
        
        [Range(0.5f, 4f)]
        public float barMovementDuration = 2f;
        
        private RectTransform _rectTransform;

        private CanvasGroup _canvasGroup;

        private float _maxHeight;

        public delegate void FinishOpenDelegate();

        public event FinishOpenDelegate OnFinishOpen;

        public delegate void FinishCloseDelegate();

        public event FinishCloseDelegate OnFinishClose;
        
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _maxHeight = _rectTransform.sizeDelta.y / 2;
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public IEnumerator CloseBars()
        {
            _canvasGroup.blocksRaycasts = true;
            
            var elapsedTime = 0f;

            while (elapsedTime <= barMovementDuration)
            {
                var newHeight = barMovementCurve.Evaluate(elapsedTime / barMovementDuration) * _maxHeight;

                SetBarHeights(newHeight);

                yield return null;
                
                elapsedTime += Time.deltaTime;
            }
            
            SetBarHeights(_maxHeight);
            
            OnFinishClose?.Invoke();
        }

        public IEnumerator OpenBars()
        {
            var elapsedTime = 0f;

            while (elapsedTime <= barMovementDuration)
            {
                var newHeight =
                    _maxHeight - barMovementCurve.Evaluate(elapsedTime / barMovementDuration) * _maxHeight;

                SetBarHeights(newHeight);

                elapsedTime += Time.deltaTime;

                yield return null;
            }
            
            SetBarHeights(0);

            _canvasGroup.blocksRaycasts = false;

            OnFinishOpen?.Invoke();
        }

        private void SetBarHeights(float newHeight)
        {
            var newSize = new Vector2(topBarTransform.sizeDelta.x, newHeight);

            topBarTransform.sizeDelta = newSize;
            bottomBarTransform.sizeDelta = newSize;
        }
    }
}