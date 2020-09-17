using PUnity.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PUnity
{
    public static class Persistent
    {
        private static GameObject _persistentGameObject;
        public static GameObject PersistentGameObject
        {
            get
            {
                if (_persistentGameObject == null)
                {
                    _persistentGameObject = new GameObject("PersistentGameObject");
                    MoveToPersistentScene(_persistentGameObject);
                }
                return _persistentGameObject;
            }
        }

        private static GameObject _componentParent;
        public static GameObject ComponentParent
        {
            get
            {
                if (_componentParent == null)
                {
                    _componentParent = new GameObject("ComponentParent");
                    MoveToPersistentScene(_componentParent);
                }
                return _componentParent;
            }
        }

        private static Scene _persistentScene;
        public static Scene PersistentScene
        {
            get
            {
                if (!_persistentScene.isLoaded)
                {
                    _persistentScene = SceneManager.GetSceneByName("PERSISTENT_SCENE");

                    if (!_persistentScene.isLoaded)
                        _persistentScene = SceneManager.CreateScene("PERSISTENT_SCENE");
                }
                return _persistentScene;
            }
        }

        public static void MoveToPersistentScene(GameObject obj)
        {
            if (obj != null)
                SceneManager.MoveGameObjectToScene(obj, PersistentScene);
            else
                Debug.LogError("Unable to move null object to persistent scene");
        }

        public static T[] FindObjectsOfType<T>() where T : Component
        {
            int index = -1;
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if(scene == PersistentScene)
                {
                    index = i;
                    break;
                }
            }

            if (index >= 0)
                return SceneFindUtility.FindAllComponentsOfType<T>(index);
            else
                return new T[0];
        }

        public static GameObject[] GetRootObjects()
        {
            return PersistentScene.GetRootGameObjects();
        }

        public static T CreateComponent<T>() where T : Component
        {
            string typeName = typeof(T).Name;
            GameObject go = new GameObject(typeName);
            T comp = go.AddComponent<T>();
            go.transform.SetParent(ComponentParent.transform);
            return comp;
        }

        public static T GetComponent<T>() where T : Component
        {
            return ComponentParent.GetComponentInChildren<T>();
        }

        public static T[] GetComponents<T>() where T : Component
        {
            return ComponentParent.GetComponentsInChildren<T>();
        }
    }
}