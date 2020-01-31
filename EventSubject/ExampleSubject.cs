//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System;
//using Pooki.Core.Events;


///// <summary>
///// Example subject shows how one can implement EventSubject to MonoBehavior
///// </summary>
//public class ExampleSubject : MonoBehaviour
//{
//    private EventSubject<ExampleEventArgs> _exampleEvents;
//    /// <summary>
//    /// EventSubject which observers can get attached to and Subject can use to send events.
//    /// </summary>
//    public EventSubject<ExampleEventArgs> ExampleEvents
//    {
//        get
//        {
//            if (_exampleEvents == null)
//                _exampleEvents = new EventSubject<ExampleEventArgs>(this, OnAttach);

//            return _exampleEvents;
//        }
//    }

//    /// <summary>
//    /// Method for EventSubject call-back that returns event arguments for observer getting attached.
//    /// I.E return current state / value of the target being observed to the observer. 
//    /// </summary>
//    /// <returns>Event arguments for attach</returns>
//    ExampleEventArgs OnAttach()
//    {
//        return ExampleEventArgs.Create(ExampleEventArgs.EventType.Attach, "Attach");
//    }

//    /// <summary>
//    /// Send test event with event arguments to all the observers.
//    /// </summary>
//    [ContextMenu("SendTestEvent")]
//    public void SendTestEvent()
//    {
//        ExampleEvents.SendEvent(ExampleEventArgs.Create(ExampleEventArgs.EventType.ExampleEvent, "Hello"));
//    }
//}

///// <summary>
///// Events for Debug console for notifying when command or message gets received.
///// </summary>
//public class ExampleEventArgs : EventArgs
//{
//    public enum EventType { Attach, ExampleEvent }
//    public EventType Event { get; private set; }
//    public string Value { get; private set; }

//    /// <summary>
//    /// Creates console event based on given parameters
//    /// </summary>
//    /// <param name="type">Event type</param>
//    /// <param name="value">message or command</param>
//    /// <returns></returns>
//    public static ExampleEventArgs Create(EventType type, string value = "")
//    {
//        ExampleEventArgs newEvent = new ExampleEventArgs();
//        newEvent.Event = type;
//        newEvent.Value = value;
//        return newEvent;
//    }
//}
