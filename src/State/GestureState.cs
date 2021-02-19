using System;
using System.Collections.Generic;

namespace State
{
    public class GestureState : IState
    {
        public IAction Action { get; set; } = new NullAction();
        public List<IState> Nexts { get; } = new List<IState>();
        public Action ReleaseAction { get; set; } = () => { };
    }
}
