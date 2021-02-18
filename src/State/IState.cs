using System.Collections.Generic;

namespace State
{
    public interface IState
    {
        public IAction Action { get; }
        public List<IState> Nexts { get; }
    }
}
