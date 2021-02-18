namespace State
{
    public class MouseMoveAction : IAction
    {
        public enum StrokeDirection
        {
            Up,
            Down,
            Left,
            Right,
        }

        public int Distance { get; set; } = 50;
        public StrokeDirection Direction { get; set; } = StrokeDirection.Left;
    }
}
