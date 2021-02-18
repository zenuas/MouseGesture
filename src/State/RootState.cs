using System.Collections.Generic;

namespace State
{
    public class RootState : IState
    {
        public IAction Action { get; } = new NullAction();
        public List<IState> Nexts { get; } = new List<IState>();
    }
}
