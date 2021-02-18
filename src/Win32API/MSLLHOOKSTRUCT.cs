using System;

namespace Win32API
{
    public struct MSLLHOOKSTRUCT
    {
        public POINT pt;
        public MOUSEDATA mouseData;
        public int flags;
        public int time;
        public UIntPtr dwExtraInfo;
    }
}
