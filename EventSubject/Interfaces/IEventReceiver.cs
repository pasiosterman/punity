namespace Pooki.Core.Events
{
    public interface IEventReceiver<T>
    {
        void HandleEvent(object sender, T args);
    }
}