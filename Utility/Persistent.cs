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
                    MoveToPersistentScene(_persistentGameObject);
                }
                return _persistentGameObject;
            }
        }

        private static Scene _persistentScene;
        public static Scene PersistentScene
        {
            get
            {
                if (_persistentScene == null)
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
            SceneManager.MoveGameObjectToScene(obj, PersistentScene);
        }

        public static T CreateComponent<T>() where T : Component
        {
            GameObject go = new GameObject(typeof(T).Name);
            T comp = go.AddComponent<T>();
            go.transform.SetParent(ComponentParent.transform);

            return comp;
        }

        public static T GetComponent<T>() where T : Component
        {
            return PersistentGameObject.GetComponentInChildren<T>();
        }

        public static T[] GetComponents<T>() where T : Component
        {
            return PersistentGameObject.GetComponentsInChildren<T>();
        }
    }
}