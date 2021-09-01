using UnityEngine;

namespace SIVS
{
    [RequireComponent(typeof(Renderer))]
    public class MaterialOffsetScroll : MonoBehaviour
    {
        [Range(-1f, 1f)]
        public float xScrollSpeed;

        [Range(-1f, 1f)]
        public float yScrollSpeed;

        private Renderer _renderer;
        
        private static readonly int mainTex = Shader.PropertyToID("_MainTex");

        private void Awake() => _renderer = GetComponent<Renderer>();

        private float _xScroll;

        private float _yScroll;

        private void FixedUpdate()
        {
            var x = Mathf.Repeat(_xScroll += xScrollSpeed * Time.fixedDeltaTime, 1);
            var y = Mathf.Repeat(_yScroll += yScrollSpeed * Time.fixedDeltaTime, 1);
            _renderer.sharedMaterial.SetTextureOffset(mainTex, new Vector2(x, y));
        }
    }
}