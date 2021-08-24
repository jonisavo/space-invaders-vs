using System.Collections;
using UnityEngine;

namespace SIVS
{
    [RequireComponent(typeof(Renderer))]
    public class PlayAreaBackgroundPlane : MonoBehaviour
    {
        private Renderer _renderer;

        private MaterialPropertyBlock _propertyBlock;

        private int _colorPropertyID = -1;

        private Color _defaultColor;

        private void Awake()
        {
            _propertyBlock = new MaterialPropertyBlock();
            _renderer = GetComponent<Renderer>();
            _colorPropertyID = Shader.PropertyToID("_Color");
            _defaultColor = _renderer.material.color;
            _renderer.GetPropertyBlock(_propertyBlock);
        }

        public void Flash(Color color, int count, float duration)
        {
            StartCoroutine(FlashCoroutine(color, count, duration));
        }

        private IEnumerator FlashCoroutine(Color color, int count, float duration)
        {
            var wait = new WaitForSeconds(duration);
            
            for (var i = 0; i < count; i++)
            {
                ChangeColor(color);
                UpdateRenderer();

                yield return wait;
                
                ChangeColor(_defaultColor);
                UpdateRenderer();

                yield return wait;
            }
        }

        private void ChangeColor(Color color) =>
            _propertyBlock.SetColor(_colorPropertyID, color);
        
        private void UpdateRenderer() =>
            _renderer.SetPropertyBlock(_propertyBlock);
    }
}