using System.Collections;

namespace SIVS
{
    public class MoveBackAndForth : MoveToMonoBehaviour
    {
        protected override IEnumerator MoveCoroutine()
        {
            while (true)
            {
                yield return MoveTo(EndPosition);
                yield return MoveTo(StartPosition);
            }
        }
    }
}