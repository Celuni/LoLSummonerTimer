using System;
using System.Runtime.InteropServices;

namespace Loltimer
{
    internal class NativeMethods
    {
        [DllImport("user32.dll")]
        internal static extern IntPtr FindWindow(String sClassName, String sAppName);

        [DllImport("user32.dll")]
        internal static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);

        [DllImport("user32.dll")]
        internal static extern bool UnregisterHotKey(IntPtr hWnd, int id);
    }
}
