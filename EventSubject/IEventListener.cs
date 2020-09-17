using System;

namespace Pooki.EventHandling
{
    public interface IEventListener<T> where T : EventArgs
    {
        void HandleEvent(object sender, T eventArgs);
    } 
}