using System;

namespace State
{
    public class AnyAction : IAction
    {
        public Func<bool> Any = () => false;
    }
}
