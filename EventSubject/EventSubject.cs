using System;

namespace Pooki.Core.Events
{
    /// <summary>
    /// Generic subject class for sending specified events.
    /// Mainly to avoid having write attach/detach and send-event methods to multiple subjects. 
    /// </summary>
    /// <typeparam name="T">EventArgs type</typeparam>
    public class EventSubject<T> where T : EventArgs
    {
        private readonly object _sender;

        /// <summary>
        /// Event to send when observer gets attached to EventSubject.
        /// </summary>
        private Func<T> _fetchAttachArgs;

        /// <summary>
        /// EventHandler for sending events to attached observers, will be null has no listeners.
        /// </summary>
        private EventHandler<T> _handler;

        public string debugID = "NOTSET";
        
        /// <summary>
        /// Constructor for event subject.
        /// </summary>
        /// <param name="sender">Sender  </param>
        /// <param name="fetchAttachArgs"></param>
        public EventSubject(object sender, Func<T> fetchAttachArgs)
        {
            _sender = sender;
            _fetchAttachArgs = fetchAttachArgs;
        }

        public EventSubject(object sender)
        {
            _sender = sender;
            _fetchAttachArgs = null;
        }

        public void Attach(IEventReceiver<T> receiver)
        {
            if (receiver == null)
            {
                UnityEngine.Debug.Log("Given receiver is null ");
                return;
            }

            _handler -= receiver.HandleEvent; //prevents duplicates
            _handler += receiver.HandleEvent;

            try
            {
                if(_fetchAttachArgs != null)
                {
                    T attachArgs = _fetchAttachArgs.Invoke();
                    if (attachArgs != null)
                    {
                        receiver.HandleEvent(_sender, attachArgs);
                    }
                }
            }
            catch (Exception e)
            {
                UnityEngine.Object context = receiver as UnityEngine.Object;
                if (context != null)
                    UnityEngine.Debug.LogError("HandleEvent method of " + receiver.GetType().Name + " caused an exception on Attach event \n" + e.Message, context);
                else
                    UnityEngine.Debug.LogError("HandleEvent method of " + receiver.GetType().Name + " caused an exception on Attach event \n" + e.Message);
            }
        }

        public void Detach(IEventReceiver<T> receiver)
        {
            _handler -= receiver.HandleEvent;
        }

        public void SendEvent(T eventArgs)
        {
            //If no observers for events are attached, simply early out.
            if (_handler == null)
            {
                return;
            }

            Delegate[] delegates = _handler.GetInvocationList();
            for (int i = 0; i < delegates.Length; i++)
            {
                try
                {
                    delegates[i].DynamicInvoke(_sender, eventArgs);
                }
                catch (Exception e)
                {
                    if (delegates[i].Target != null)
                    {
                        string targetType = delegates[i].Target.GetType().Name;

                        UnityEngine.Object context = delegates[i].Target as UnityEngine.Object;

                        if (context != null)
                            UnityEngine.Debug.LogError("Error in the " + context.name + " - " + targetType + " HandleEvent method. \n" + e, context);
                        else
                            UnityEngine.Debug.LogError("Error in the " + targetType + " HandleEvent method. \n" + e);
                    }
                    else
                    {
                        UnityEngine.Debug.LogError("Error in the handler " + _handler.Method.Name + "\n" + e);
                    }
                }
            }
        }
    }
}