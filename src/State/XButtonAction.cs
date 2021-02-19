using Win32API;

namespace State
{
    public class XButtonAction : IAction
    {
        public WM_MESSAGE Message { get; set; } = WM_MESSAGE.WM_XBUTTONUP;
        public short Button { get; set; } = XBUTTON.XBUTTON1;
    }
}
