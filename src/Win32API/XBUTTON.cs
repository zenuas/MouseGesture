using System.Runtime.InteropServices;

namespace Win32API
{
    [StructLayout(LayoutKind.Sequential)]
    public struct XBUTTON
    {
        public short reserved_;
        public short type;

        public const short XBUTTON1 = 0x0001;
        public const short XBUTTON2 = 0x0002;
    }
}
