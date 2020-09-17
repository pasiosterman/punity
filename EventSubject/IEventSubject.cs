using System;
using System.Collections.Generic;

namespace PUnity.EventHandling
{
    public interface IEventSubject<T> where T : EventArgs
    {
        void Attach(IEventListener<T> listener);
        void Detach(IEventListener<T> listener);
        void SendEvent(T args);
    }
}