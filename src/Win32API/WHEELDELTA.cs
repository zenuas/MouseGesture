using System.Runtime.InteropServices;

namespace Win32API
{
    [StructLayout(LayoutKind.Sequential)]
    public struct WHEELDELTA
    {
        public short reserved_;
        public short delta;

        public const int WHEEL_DELTA = 120;
    }
}
