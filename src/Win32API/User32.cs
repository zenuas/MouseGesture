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

        [DllImport("user32", EntryPoint = "GetAsyncKeyState")]
        public static extern short GetAsyncKeyState_(int vKey);
        public static bool GetAsyncKeyState(VK_VIRTUALKEY vKey) => GetAsyncKeyState_((int)vKey) < 0;
    }
}
