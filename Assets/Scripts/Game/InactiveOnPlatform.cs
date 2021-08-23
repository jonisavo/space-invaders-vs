namespace SIVS
{
    public class InactiveOnPlatform : PlatformMonoBehaviour
    {
        protected override void OnAwake() => gameObject.SetActive(false);
    }
}
