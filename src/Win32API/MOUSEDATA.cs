using System.Runtime.InteropServices;

namespace Win32API
{
    [StructLayout(LayoutKind.Explicit)]
    public struct MOUSEDATA
    {
        [FieldOffset(0)] public WHEELDELTA wheeldelta;
        [FieldOffset(0)] public XBUTTON xbutton;
    }
}
