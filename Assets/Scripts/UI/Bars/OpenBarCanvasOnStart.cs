using UnityEngine;

namespace SIVS
{
    [RequireComponent(typeof(BarCanvas))]
    public class OpenBarCanvasOnStart : MonoBehaviour
    {
        private void Start()
        {
            StartCoroutine(GetComponent<BarCanvas>().OpenBars());
        }
    }
}
