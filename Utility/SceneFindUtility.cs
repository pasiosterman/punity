//Copyright 2020 Pasi Österman

//Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the 
//Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software,
//and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR 
//ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH 
//THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PUnity.Utils
{
    public static class SceneFindUtility
    {
        public static T[] FindAllComponentsOfType<T>(int sceneIndex = -1) where T : Component
        {
            List<GameObject> rootObjects = new List<GameObject>();

            if(sceneIndex < 0)
            {
                //Get all scenes
                int count = SceneManager.sceneCount;
                Scene[] allScenes = new Scene[count];
                for (int i = 0; i < count; i++)
                {
                    Scene scene = SceneManager.GetSceneAt(i);
                    allScenes[i] = scene;
                }

                //Get all root objects
                for (int i = 0; i < allScenes.Length; i++)
                {
                    if (!allScenes[i].isLoaded)
                    {
                        Debug.LogError(allScenes[i].name + " not loaded");
                        continue;
                    }
                    rootObjects.AddRange(allScenes[i].GetRootGameObjects());
                }
            }
            else
            {
                Scene scene = SceneManager.GetSceneAt(sceneIndex);
                if(scene == null || !scene.IsValid())
                {
                    Debug.LogError("Scene with index" + sceneIndex + " not currently loaded");
                    return new T[0];
                }
                rootObjects.AddRange(scene.GetRootGameObjects());
            }

            //Get all taggedobjects
            List<T> foundComponents = new List<T>();
            for (int i = 0; i < rootObjects.Count; i++)
            {
                T[] arr = rootObjects[i].GetComponentsInChildren<T>(true);
                foundComponents.AddRange(arr);
            }

            return foundComponents.ToArray();
        }
    }
}