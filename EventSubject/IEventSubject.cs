using System;
using System.Collections.Generic;

namespace Pooki.EventHandling
{
    public interface IEventSubject<T> where T : EventArgs
    {
        void Attach(IEventListener<T> listener);
        void Detach(IEventListener<T> listener);
        void SendEvent(T args);
    }
}