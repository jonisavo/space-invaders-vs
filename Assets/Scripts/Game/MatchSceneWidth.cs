using UnityEngine;

namespace SIVS
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public class MatchSceneWidth : MonoBehaviour
    {
        [Tooltip("the in-world distance between the left & right edges of your scene.")]
        public float sceneWidth = 10;

        private void Awake() {
            var camera = GetComponent<Camera>();
            
            var unitsPerPixel = sceneWidth / Screen.width;

            var desiredHalfHeight = 0.5f * unitsPerPixel * Screen.height;

            camera.orthographicSize = desiredHalfHeight;
        }
    }
}