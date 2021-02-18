using System;
using System.Collections.Generic;

namespace State
{
    public class HoldState : IState
    {
        public IAction Action { get; set; } = new NullAction();
        public IAction Release { get; set; } = new NullAction();
        public List<IState> Nexts { get; } = new List<IState>();
        public Action NoActionRelease { get; set; } = () => { };
    }
}
