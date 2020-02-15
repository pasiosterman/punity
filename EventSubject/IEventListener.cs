using System;

namespace Forbidden.EventHandling
{
    public interface IEventListener<T> where T : EventArgs
    {
        void HandleEvent(object sender, T eventArgs);
    } 
}