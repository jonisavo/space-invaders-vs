using UnityEngine;

namespace SIVS
{
    public class ChangeTransformOnPlatform : PlatformMonoBehaviour
    {
        [Header("Transform")]
        public bool changePosition;
        
        public Vector3 position;

        [Header("RectTransform")]
        public bool changeWidthAndHeight;
        
        public float rectTransformWidth;

        public float rectTransformHeight;
        
        protected override void OnAwake()
        {
            if (changePosition)
                transform.position = position;
            
            if (transform is RectTransform rectTransform)
                ChangeRectTransform(rectTransform);
        }

        private void ChangeRectTransform(RectTransform rectTransform)
        {
            if (changeWidthAndHeight)
                rectTransform.sizeDelta = new Vector2(rectTransformWidth, rectTransformHeight);
        }
    }
}