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

        private Coroutine _flashCoroutine;

        private void Awake()
        {
            _propertyBlock = new MaterialPropertyBlock();
            _renderer = GetComponent<Renderer>();
            _colorPropertyID = Shader.PropertyToID("_Color");
            _defaultColor = _renderer.material.color;
            _renderer.GetPropertyBlock(_propertyBlock);
        }

        private void OnEnable() => FreezeFrame.OnTrigger += StopFlashing;

        private void OnDisable() => FreezeFrame.OnTrigger -= StopFlashing;

        public void Flash(Color color, int count, float duration)
        {
            if (_flashCoroutine != null)
                StopCoroutine(_flashCoroutine);
            
            _flashCoroutine = StartCoroutine(FlashCoroutine(color, count, duration));
        }

        public void StopFlashing()
        {
            if (_flashCoroutine == null)
                return;

            StopCoroutine(_flashCoroutine);
            
            ChangeColor(_defaultColor);
            UpdateRenderer();

            _flashCoroutine = null;
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

            _flashCoroutine = null;
        }

        private void ChangeColor(Color color) =>
            _propertyBlock.SetColor(_colorPropertyID, color);
        
        private void UpdateRenderer() =>
            _renderer.SetPropertyBlock(_propertyBlock);
    }
}