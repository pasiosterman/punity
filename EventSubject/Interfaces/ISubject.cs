namespace Pooki.Core.Events
{
    public interface IEventSender<T>
    {
        void Attach(IEventReceiver<T> observer);
        void Detach(IEventReceiver<T> observer);
    } 
}