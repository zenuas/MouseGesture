using System;
using System.Runtime.InteropServices;

namespace Win32API
{
    public static class User32
    {
        public delegate int HookProc(HOOKCODES nCode, WM_MESSAGE wParam, IntPtr lParam);

        [DllImport("user32")]
        public static extern IntPtr SetWindowsHookEx(SETWINDOWSHOOKCODES idHook, HookProc lpfn, IntPtr hInstance, int threadId);

        [DllImport("user32")]
        public static extern int CallNextHookEx(IntPtr hhk, HOOKCODES nCode, WM_MESSAGE wParam, IntPtr lParam);
    }
}
