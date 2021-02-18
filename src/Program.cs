using Extensions;
using State;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Win32API;

namespace MouseGesture
{
    public static class Program
    {
        static IntPtr NextMouseHook;
        readonly static RootState Root = new RootState();
        static HoldState? TopState = null;
        static IState CurrentState = Root;
        static Point LastCursorPosition = new Point();
        static bool EventFired = false;

        [STAThread]
        static void Main(string[] args)
        {
            _ = Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var r = new HoldState
            {
                Action = new MessageAction { Message = WM_MESSAGE.WM_RBUTTONDOWN },
                Release = new MessageAction { Message = WM_MESSAGE.WM_RBUTTONUP },
                NoActionRelease = () => Debug.WriteLine("RButton NoAction")
            };
            r.Nexts.Add(new LockerState { Action = new WheelAction { Direction = WheelAction.WheelDirection.Up }, Fire = () => Debug.WriteLine("Wheel Up") });
            r.Nexts.Add(new LockerState { Action = new WheelAction { Direction = WheelAction.WheelDirection.Down }, Fire = () => Debug.WriteLine("Wheel Down") });
            r.Nexts.Add(new GestureState { Action = new MouseMoveAction { Direction = MouseMoveAction.StrokeDirection.Up }, Fire = () => Debug.WriteLine("Move Up") });
            r.Nexts.Add(new GestureState { Action = new MouseMoveAction { Direction = MouseMoveAction.StrokeDirection.Down }, Fire = () => Debug.WriteLine("Move Down") });
            r.Nexts.Add(new GestureState { Action = new MouseMoveAction { Direction = MouseMoveAction.StrokeDirection.Left }, Fire = () => Debug.WriteLine("Move Left") });
            r.Nexts.Add(new GestureState { Action = new MouseMoveAction { Direction = MouseMoveAction.StrokeDirection.Right }, Fire = () => Debug.WriteLine("Move Right") });
            Root.Nexts.Add(r);

            var hInstance = Kernel32.GetModuleHandle(null);
            NextMouseHook = User32.SetWindowsHookEx(SETWINDOWSHOOKCODES.WH_MOUSE_LL, MouseHookProc, hInstance, 0);

            Application.Run(new Form());
        }

        static int MouseHookProc(HOOKCODES nCode, WM_MESSAGE wParam, IntPtr lParam)
        {
            if (nCode >= HOOKCODES.HC_ACTION)
            {
                List<IState>? nexts = null;
                switch (wParam)
                {
                    case WM_MESSAGE.WM_MOUSEWHEEL:
                    case WM_MESSAGE.WM_XBUTTONDOWN:
                    case WM_MESSAGE.WM_XBUTTONUP:
                    case WM_MESSAGE.WM_XBUTTONDBLCLK:
                    case WM_MESSAGE.WM_NCXBUTTONDOWN:
                    case WM_MESSAGE.WM_NCXBUTTONUP:
                    case WM_MESSAGE.WM_NCXBUTTONDBLCLK:

                        var p = PtrToStructure<MSLLHOOKSTRUCT>(lParam);
                        nexts = wParam == WM_MESSAGE.WM_MOUSEWHEEL
                            ? CurrentState.Nexts.Where(x => ChooseAction(x.Action, DeltaToWheelDirection(p.mouseData.wheeldelta.delta))).ToList()
                            : CurrentState.Nexts.Where(x => ChooseAction(x.Action, wParam, p.mouseData.xbutton.type)).ToList();
                        break;

                    case WM_MESSAGE.WM_MOUSEMOVE:
                        nexts = CurrentState.Nexts.Where(x => ChooseAction(x.Action, LastCursorPosition, Cursor.Position)).ToList();
                        break;

                    default:
                        nexts = CurrentState.Nexts.Where(x => ChooseAction(x.Action, wParam)).ToList();
                        break;
                }
                if ((nexts is null || nexts.Count == 0) && TopState is { } && ChooseAction(TopState.Release, wParam))
                {
                    if (ReleaseState()) return 1;
                }
                else if (nexts is { } && nexts.Count > 0)
                {
                    if (TopState is null) TopState = nexts.First().Cast<HoldState>();
                    var prev = LastCursorPosition;
                    LastCursorPosition = Cursor.Position;

                    var continued = false;
                    nexts.By<IEvent>().Each(x => { EventFired = true; x.Fire(); continued = x is LockerState || continued; });
                    if (!continued) CurrentState = nexts.First();
                    return 1;
                }
            }
            return User32.CallNextHookEx(NextMouseHook, nCode, wParam, lParam);
        }

        public static bool ReleaseState()
        {

            var fired = EventFired;
            var istop = TopState == CurrentState;
            if (!fired && istop) TopState!.NoActionRelease();
            CurrentState = Root;
            TopState = null;
            EventFired = false;
            return fired;
        }

        public static bool ChooseAction(IAction x, WM_MESSAGE m) => x is NullAction || (x is MessageAction msg && msg.Message == m);

        public static bool ChooseAction(IAction x, Point prev, Point current) => x is NullAction || (x is MouseMoveAction move && move.Distance * move.Distance <= DistanceSquare(prev, current) && move.Direction == DirectionAtoB(prev, current));

        public static bool ChooseAction(IAction x, WheelAction.WheelDirection d) => x is NullAction || (x is WheelAction wheel && wheel.Direction == d);

        public static bool ChooseAction(IAction x, WM_MESSAGE m, short type) => x is NullAction || (x is XButtonAction xb && xb.Message == m && xb.Button == type);

        public static T PtrToStructure<T>(IntPtr p) => Marshal.PtrToStructure(p, typeof(T))!.Cast<T>();

        public static int DistanceSquare(Point a, Point b) => (int)(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));

        public static MouseMoveAction.StrokeDirection DirectionAtoB(Point a, Point b)
        {
            var d = (int)(Math.Atan2(b.Y - a.Y, b.X - a.X) * 180 / Math.PI);
            return 135 <= d || d < -135 ? MouseMoveAction.StrokeDirection.Left
                : -45 <= d && d < 45 ? MouseMoveAction.StrokeDirection.Right
                : 45 <= d && d < 135 ? MouseMoveAction.StrokeDirection.Down
                : MouseMoveAction.StrokeDirection.Up;
        }

        public static WheelAction.WheelDirection DeltaToWheelDirection(int delta) => delta < 0 ? WheelAction.WheelDirection.Down : WheelAction.WheelDirection.Up;
    }
}
