using System;

namespace State
{
    public interface IEvent
    {
        public Action Fire { get; }
    }
}
