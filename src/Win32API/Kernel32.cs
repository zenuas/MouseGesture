using System;
using System.Runtime.InteropServices;

namespace Win32API
{
    public static class Kernel32
    {
        [DllImport("kernel32")]
        public static extern IntPtr GetModuleHandle(string? name);
    }
}
