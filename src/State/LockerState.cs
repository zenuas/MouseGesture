using System;
using System.Collections.Generic;

namespace State
{
    public class LockerState : IState, IEvent
    {
        public IAction Action { get; set; } = new NullAction();
        public List<IState> Nexts { get; } = new List<IState>();
        public Action Fire { get; set; } = () => { };
    }
}
