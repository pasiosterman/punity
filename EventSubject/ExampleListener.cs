//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Pooki.Core.Events;

//public class ExampleListener : MonoBehaviour, IObserver<ConsoleEventArgs>
//{
//    public ExampleSubject subject;

//    public void OnEnable()
//    {
//        subject.ExampleEvents.Attach(this);
//    }

//    public void HandleEvent(object sender, ConsoleEventArgs args)
//    {
//        Debug.Log(args.Value);
//    }
//}