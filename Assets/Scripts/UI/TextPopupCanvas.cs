using UnityEngine;

namespace SIVS
{
    [RequireComponent(typeof(Canvas))]
    public class TextPopupCanvas : MonoBehaviour
    {
        private void OnEnable() => TextPopup.OnSpawn += OnTextPopupSpawn;

        private void OnDisable() => TextPopup.OnSpawn -= OnTextPopupSpawn;

        private void OnTextPopupSpawn(TextPopup textPopup)
        {
            var popupTransform = textPopup.transform;
            
            popupTransform.SetParent(transform, true);

            var position = popupTransform.position;

            popupTransform.position = new Vector3(position.x, position.y, -2f);
        }
    }
}