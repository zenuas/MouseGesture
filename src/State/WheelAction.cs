namespace State
{
    public class WheelAction : IAction
    {
        public enum WheelDirection
        {
            Up,
            Down,
        }

        public WheelDirection Direction { get; set; } = WheelDirection.Up;
    }
}
