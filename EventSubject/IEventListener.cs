using System;

namespace PUnity.EventHandling
{
    public interface IEventListener<T> where T : EventArgs
    {
        void HandleEvent(object sender, T eventArgs);
    } 
}