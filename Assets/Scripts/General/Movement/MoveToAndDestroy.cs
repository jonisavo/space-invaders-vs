using System.Collections;

namespace SIVS
{
    public class MoveToAndDestroy : MoveToMonoBehaviour
    {
        protected override IEnumerator MoveCoroutine()
        {
            yield return MoveTo(EndPosition);
            
            Destroy(gameObject);
        }
    }
}