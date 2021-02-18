using Win32API;

namespace State
{
    public class MessageAction : IAction
    {
        public WM_MESSAGE Message { get; set; } = WM_MESSAGE.WM_RBUTTONDOWN;
    }
}
