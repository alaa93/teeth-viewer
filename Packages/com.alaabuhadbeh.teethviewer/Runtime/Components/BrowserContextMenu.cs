#if UNITY_WEBGL && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif

namespace AlaAbuhadbeh.TeethViewer
{
    public static class BrowserContextMenu
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void TeethViewer_DisableContextMenu();

        public static void Disable() => TeethViewer_DisableContextMenu();
#else
        public static void Disable() { }
#endif
    }
}
