# PUnity
Repository for common unity scripts that I use in my game projects. 

1. [Event Handling](#Event-Handling)
2. [Serialization](#Serialization)
3. [ToolBox](#ToolBox)
4. [Persistent](#Persistent)

## Event Handling
Classes and Interfaces for handling game events with EventArgs in Unity. Classes that need to send events Implement __IEventSubject__ interface and use the __GenericEventHandler__ to send events as well as attach and remove listeners. Classes that listen to events implement __IEventListener__ Interface and branch their logic based on the type of the recieved event.

### Example EventSubject
Player MonoBehavior that sends OnDeathEventArgs when hit by any collider. 

```csharp
using UnityEngine;
using PUnity.EventHandling;

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
using PUnity.EventHandling;


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
using PUnity.Serialization;

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
Class for aggregating singleton classes and removing them of the responsibility of actually being singletons. Use in with interfaces to further reduce the coupling in your game or application. 

With ToolBox pattern you'll be calling singletons through static class like GameTool that contains properties for all the would be singletons in your game. So instead of __ClassName.Instance.Method__ you'll be calling __GameTool.Tool.Method__. Benefit for this is that it's as simple to use as singleton but you retain the ability to have multiple instances of the class as well as ability to easily change the class returned by the property with another sharing the same interface/methods. 

### Example ToolBox

Make static class and add the Generic lazy-init ToolBox property. Add static properties for whatever class instances you want to use as tools. These properties can be lazy-initialized or you can make separate init method you call at the start of your game. Latter can be handy if initialization of tool loads any resources that may cause fps spikes. 

In the example GameManager property is Unity [Component](https://docs.unity3d.com/ScriptReference/Component.html) so it needs to be added to a GameObject. Persistent.CreateComponent is used for that to create GameObject with given component attached to a persistent scene. You can use [Object.DontDestroyOnLoad](https://docs.unity3d.com/ScriptReference/Object.DontDestroyOnLoad.html) instead as well.

```csharp
using UnityEngine;
using PUnity;

public static class GameTool
{
    private static ToolBox<GameTools> _toolbox;
    static ToolBox<GameTools> ToolBox
    {
        get
        {
            if (_toolbox == null)
                _toolbox = new ToolBox<GameTools>();

            return _toolbox;
        }
    }

    public static SaveSystem SaveSystem
    {
        get
        {
            if(!ToolBox.ContainsTool(GameTools.SaveSystem))
                ToolBox.SetTool(GameTools.SaveSystem, new SaveSystem());

            return ToolBox.GetTool<SaveSystem>(GameTools.SaveSystem);
        }
    }

    public static GameManager GameManager
    {
        get
        {
            if (!ToolBox.ContainsTool(GameTools.SaveSystem))
            {
                GameManager gameManager = Persistent.CreateComponent<GameManager>();
                ToolBox.SetTool(GameTools.GameManager, GameManager);
            }
            return ToolBox.GetTool<GameManager>(GameTools.GameManager);
        }
    }
}

public enum GameTools
{
    GameManager = 1,
    SaveSystem = 2,
}
```
## Persistent

Games often need persistent GameObjects that do not get unloaded during sceneload. __Persistent__ is a static class that creates a scene on runtime for storing and accessing persistent objects. It's designed to be used with __SceneLoader(WIP)__ that uses Unitys [LoadSceneMode.Additive](https://docs.unity3d.com/ScriptReference/SceneManagement.LoadSceneMode.html) feature hence it wont work with __LoadSceneMode.Single__. If you have your own SceneLoader you need to set it not to unload persistent scene. 


### Things one might place to persistent scene
1. SceneLoader and loading screen
    - Pretty much required for smooth animated loading screen.
2. User interface
    - To avoid having to reload potentually high resolution assets on scene load.
3. Object Pooling
    - Avoid having to reload high fidelity assets for Bullets, explosions, waypoints etc.
4. Audio Player
    - Background music for loading screen, fading tracks on scene transition etc.
5. SaveSystem
    - No need to reload player save every time scene changes. 

### Example usage

Use __Persistent.CreateComponent\<ComponentType\>__ to add new GameObject with given component to persistent scene. __Persistent.MoveToPersistentScene__ can be used to move given GameObject to Persistent scene. __Persistent.PersistentGameObject__ quick access gameobject to parent stuff in persistent scene. __Persistent.GetRootObjects__ to get all RootGameObjects from persistent scene. 


```csharp
using UnityEngine;
using PUnity;

public class PersistentExample : MonoBehaviour
{
    void Start()
    {
        ExampleComponent comp = Persistent.CreateComponent<ExampleComponent>();

        GameObject WayPointParent = new GameObject("WayPointParent");
        Persistent.MoveToPersistentScene(WayPointParent);
        for (int i = 0; i < 10; i++)
        {
            GameObject waypoint = new GameObject("Waypoint"+ (i + 1));
            waypoint.transform.SetParent(WayPointParent.transform);
        }

        GameObject go2 = new GameObject();
        go2.transform.SetParent(Persistent.PersistentGameObject.transform);

        GameObject[] rootObjects = Persistent.GetRootObjects(); 
    }
}
```

## SceneManagement

WIP / Incomplete Asyncronious Scene loader for Unity. 