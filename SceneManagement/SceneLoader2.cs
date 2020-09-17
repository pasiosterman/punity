using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace PUnity
{
    public class SceneLoader2
    {
        HashSet<string> _systemScenes = new HashSet<string>();
        private HashSet<string> SystemScenes { get { return _systemScenes; } }

        //public static void LoadScene(string sceneName)
        //{
        //    if (loadingTask2 != null && loadingTask2.Status == TaskStatus.Running)
        //        return;

        //    loadingTask2 = LoadSceneWaitTask(sceneName);
        //}

        async Task LoadAdditiveScenesTask(string[] sceneNames)
        {
            Debug.Log(LogTags.SYSTEM + "Loading " + sceneNames.Length + " scenes");

            for (int i = 0; i < sceneNames.Length; i++)
                await LoadAdditiveSceneTask(sceneNames[i]);

            Debug.Log(LogTags.SYSTEM + "Loaded " + sceneNames.Length + " scenes");
        }

        async Task LoadAdditiveSceneTask(string sceneName)
        {
            try
            {
                Debug.Log(LogTags.SYSTEM + "Loading " + sceneName + " scene");

                AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                while (!operation.isDone)
                    await Task.Yield();

                Debug.Log(LogTags.SYSTEM + "Loaded " + sceneName + " scene");
            }
            catch (System.Exception e)
            {
                Debug.Log(LogTags.SYSTEM_ERROR + "SceneLoad resulted in error: " + e.Message);
            }
        }

    } 
}
