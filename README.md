# PookiCore
Repository for common unity scripts that I use in my game projects.

## EventSubject
Classes and Interfaces for handling game events with EventArgs in Unity. Classes that need to send events Implement __IEventSubject__ interface and use the __GenericEventHandler__ to send events as well as attach and remove listeners. Classes that listen to events implement __IEventListener__ Interface and branch their logic based on the type of the recieved event.

### Example EventSubject
```csharp
using UnityEngine;
using Pooki.EventHandling;

public class Player : MonoBehaviour, IEventSubject<PlayerEventArgs>
{
    private void OnCollisionEnter(Collision collision)
    {
        SendEvent(new OnDeathEventArgs(collision.collider.gameObject));
    }

    public void Attach(IEventListener<PlayerEventArgs> listener)
    {
        ExampleEventHandler.Attach(listener);
    }

    public void Detach(IEventListener<PlayerEventArgs> listener)
    {
        ExampleEventHandler.Detach(listener);
    }

    public void SendEvent(PlayerEventArgs args)
    {
        ExampleEventHandler.SendEvent(args);
    }

    GenericEventHandler<PlayerEventArgs> _exampleEventHandler;
    public GenericEventHandler<PlayerEventArgs> ExampleEventHandler
    {
        get
        {
            if (_exampleEventHandler == null)
                _exampleEventHandler = new GenericEventHandler<PlayerEventArgs>(this);

            return _exampleEventHandler;
        }
    }
}

public class PlayerEventArgs : System.EventArgs { }

public class OnDeathEventArgs : PlayerEventArgs
{
    public OnDeathEventArgs(GameObject killedBy) { KilledBy = killedBy; }
    public readonly GameObject KilledBy;
}
```

### Example EventListener