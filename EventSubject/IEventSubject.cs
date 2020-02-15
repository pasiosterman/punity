using System;
using System.Collections.Generic;

namespace Forbidden.EventHandling
{
    public interface IEventSubject<T> where T : EventArgs
    {
        void Attach(IEventListener<T> newListener);
        void Detach(IEventListener<T> removeListener);
        void SendEvent(T args);
    }
}