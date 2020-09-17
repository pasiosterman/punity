﻿using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace Pooki.Core
{
    /// <summary>
    /// Class for aggregating singleton classes and removing them of the responsibility 
    /// of actually being singletons. Use in conjunction with dependency injection and interfaces 
    /// to further reduce the cohension or coupling in your application.
    /// based on https://www.ibm.com/developerworks/webservices/library/co-single/index.html
    /// </summary>
    public class ToolBox
    {
        protected static ToolBox _instance;
        protected Dictionary<int, object> Tools;

        public static ToolBox Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ToolBox();

                return _instance;
            }
        }

        public static bool ContainsTool(int key)
        {
            if (Instance.Tools.ContainsKey(key))
                return true;
            else
                return false;
        }

        private ToolBox()
        {
            Tools = new Dictionary<int, object>();
        }

        public static void RegisterTool(int key, object tool)
        {
            if (Instance.Tools.ContainsKey(key))
            {
                Debug.LogWarning("Key already found, overwriting " + tool.GetType().Name);
                Instance.Tools[key] = tool;
            }
            else
            {
                Instance.Tools.Add(key, tool);
            }
        }
        public static void DeRegisterTool(int key)
        {
            if (Instance != null)
            {
                Instance.Tools.Remove(key);
            }
        }
        public static void ClearTools()
        {
            if (Instance != null)
            {
                Instance.Tools.Clear();
            }
        }

        public static object GetTool(int key)
        {
            if (Instance.Tools.ContainsKey(key))
                return Instance.Tools[key];

            else { return null; }
        }

        private static GameObject _persistentGameObject;
        public static GameObject PersistentGameObject
        {
            get
            {
                if (_persistentGameObject == null)
                {
                    _persistentGameObject = new GameObject("PersistentGameObject");
                    SceneManager.MoveGameObjectToScene(_persistentGameObject, PersistentScene);
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
    }
}