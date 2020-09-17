# PookiCore
Repository for common unity scripts that I use in my game projects. 

## EventHandling
Classes and Interfaces for handling game events with EventArgs in Unity. Classes that need to send events Implement __IEventSubject__ interface and use the __GenericEventHandler__ to send events as well as attach and remove listeners. Classes that listen to events implement __IEventListener__ Interface and branch their logic based on the type of the recieved event.

### Example EventSubject
Player MonoBehavior that sends OnDeathEventArgs when hit by any collider. 

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

GameManager that find player on enable and attaches itself to listen to any PlayerEventArgs it might send. 
```csharp
using UnityEngine;
using Pooki.EventHandling;


public class GameManager : MonoBehaviour, IEventListener<PlayerEventArgs>
{
    public bool GameOver = false;

    private void OnEnable()
    {
        ActivePlayer = FindObjectOfType<Player>();
        if (ActivePlayer != null)
            ActivePlayer.Attach(this);
    }

    private void OnDisable()
    {
        if (ActivePlayer != null)
            ActivePlayer.Detach(this);
    }

    
    public void SetGameOver()
    {
        if (!GameOver)
            GameOver = true;
    }

    public void HandleEvent(object sender, PlayerEventArgs eventArgs)
    {
        if(eventArgs.GetType() == typeof(OnDeathEventArgs))
        {
            if (!GameOver)
                SetGameOver();
        }
    }

    public Player ActivePlayer { get; private set; }
}
```

## Serialization
Reflection based JSON serializer for POCO's using SimpleJSON. For projects that can't use Json.NET for some reason. 

```csharp
using Pooki.Serialization;

public class Example
{
    public void SaveData()
    {
        SaveData data = new SaveData() { playerName = "example", level = 10, money = 150 };
        string jsonString = ObjectSerializer.SerializeObject(data);

        SaveData loadedData = ObjectDeserializer.DeserializeObject<SaveData>(jsonString);
    }
}
```

## ToolBox 
Class for aggregating singleton classes and removing them of the responsibility of actually being singletons. Use in conjunction with dependency injection and interfaces to further reduce the cohension or coupling in your application.


    

## SceneManagement
WIP / Incomplete Asyncronious scene loader for Unity. 