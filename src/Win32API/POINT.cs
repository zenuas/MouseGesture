using System.Runtime.InteropServices;

namespace Win32API
{
    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int x;
        public int y;
    }
}
