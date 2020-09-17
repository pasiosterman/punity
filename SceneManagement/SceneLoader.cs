using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace PUnity
{
    public class SceneLoader : MonoBehaviour
    {
        private readonly HashSet<string> _systemScenes = new HashSet<string>(new string[]{"PERSISTENT_SCENE"});
        private HashSet<string> SystemScenes { get { return _systemScenes; } }
        private List<IEnumerator> _routines = new List<IEnumerator>();

        //used to track progress of loading / unloading process
        int _totalSteps = 1;
        int _stepsDone = 0;
        float _currentProgress = 0.0f;

        Coroutine _processRoutine;

        public void LoadScene(string sceneID, bool unloadNonSystemScenes = false, bool setAsActiveScene = false)
        {
            CancelSceneLoadIfInProcess();

            _totalSteps = 0;

            if (unloadNonSystemScenes)
            {
                _routines.Add(UnloadAllLoadedNonSystemScenesRoutine());
                _totalSteps += NonSystemSceneCount;
            }

            _routines.Add(LoadAdditiveSceneRoutine(sceneID));

            if(setAsActiveScene)
                _routines.Add(SetActiveSceneRoutine(sceneID));

            _totalSteps++;

            _processRoutine = StartCoroutine(ProcessRoutines());
        }

        public void LoadScenes(string[] scenesToLoad, bool unloadNonSystemScenes = true)
        {
            CancelSceneLoadIfInProcess();

            _totalSteps = 0;

            if (unloadNonSystemScenes)
            {
                _routines.Add(UnloadAllLoadedNonSystemScenesRoutine());
                _totalSteps += NonSystemSceneCount;
            }

            _routines.Add(LoadAdditiveScenesRoutine(scenesToLoad));
            _routines.Add(SetActiveSceneRoutine(scenesToLoad[0]));

            _totalSteps += scenesToLoad.Length;

            _processRoutine = StartCoroutine(ProcessRoutines());
        }

        public void CancelSceneLoadIfInProcess()
        {
            if (InProgress)
            {
                Debug.LogWarning("Scene load interrupted!");
                if (_processRoutine != null)
                    StopCoroutine(_processRoutine);

                _routines.Clear();
                CurrentState = SceneLoaderStates.Idle;
            }
        }

        IEnumerator ProcessRoutines()
        {
            while(_routines.Count > 0)
            {
                IEnumerator routine = _routines[0];

                while (routine.MoveNext())
                    yield return null;

                _routines.RemoveAt(0);
            }
            CurrentState = SceneLoaderStates.Idle;
        }

        IEnumerator LoadAdditiveScenesRoutine(string[] scenesToLoadArray)
        {
            for (int i = 0; i < scenesToLoadArray.Length; i++)
            {
                IEnumerator loadRoutine = LoadAdditiveSceneRoutine(scenesToLoadArray[i]);
                while (loadRoutine.MoveNext()) yield return null;
            }
        }

        IEnumerator UnloadAllLoadedNonSystemScenesRoutine()
        {
            Scene[] loadedScenes = GetAllLoadedScenes();

            for (int i = 0; i < loadedScenes.Length; i++)
            {
                if (SystemScenes.Contains(loadedScenes[i].name))
                    continue;

                IEnumerator loadRoutine = UnloadSceneRoutine(loadedScenes[i].name);
                while (loadRoutine.MoveNext()) yield return null;
            }
        }

        IEnumerator LoadAdditiveSceneRoutine(string sceneID)
        {
            Debug.Log(GetType().Name + " Loading Scene " + sceneID, this);
            CurrentState = SceneLoaderStates.Loading;
            if (!IsSceneLoaded(sceneID))
            {
                AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneID, LoadSceneMode.Additive);
                while (!asyncLoad.isDone)
                {
                    _currentProgress = asyncLoad.progress;
                    yield return null;
                }
                _stepsDone++;
                Debug.Log(GetType().Name + " Loaded scene : " + sceneID, this);
            }
            else
            {
                //Scene already loaded, skip
                yield return null;
            }
        }

        IEnumerator UnloadSceneRoutine(string sceneID)
        {
            Debug.Log(GetType().Name + " Unloading Scene " + sceneID, this);
            CurrentState = SceneLoaderStates.Unloading;
            if (IsSceneLoaded(sceneID))
            {
                AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(sceneID);
                while (!asyncUnload.isDone)
                {
                    _currentProgress = asyncUnload.progress;
                    yield return null;
                }
                _stepsDone++;
                Debug.Log(GetType().Name + " Unloaded scene : " + sceneID, this);
            }
            else
            {
                Debug.LogWarning(GetType().Name + "Failed to unload scene with ID: " + sceneID + " as no such scene currently loaded", this);
                yield return null;
            }
        }

        IEnumerator SetActiveSceneRoutine(string sceneID)
        {
            yield return null;

            Scene scene = SceneManager.GetSceneByName(sceneID);
            SceneManager.SetActiveScene(scene);
        }

        public Scene[] GetAllLoadedScenes()
        {
            Scene[] loadedScenes = new Scene[SceneManager.sceneCount];
            for (int i = 0; i < loadedScenes.Length; i++)
                loadedScenes[i] = SceneManager.GetSceneAt(i);

            return loadedScenes;
        }

        public bool IsSceneLoaded(string name)
        {
            Scene[] allLoadedScenes = GetAllLoadedScenes();
            for (int i = 0; i < allLoadedScenes.Length; i++)
            {
                if (allLoadedScenes[i].name != name)
                    continue;

                if (allLoadedScenes[i].isLoaded)
                    return true;
            }
            return false;
        }

        public void AddSystemScene(string sceneID){ SystemScenes.Add(sceneID); }
        public void RemoveSystemScene(string sceneID){ SystemScenes.Remove(sceneID); }

        private SceneLoaderStates _currentState = SceneLoaderStates.Idle;
        public SceneLoaderStates CurrentState
        {
            get { return _currentState; }
            private set { _currentState = value; }
        }

        private int NonSystemSceneCount
        {
            get
            {
                Scene[] loadedScenes = GetAllLoadedScenes();

                int nonSystemScenes = 0;
                for (int i = 0; i < loadedScenes.Length; i++)
                {
                    if (!SystemScenes.Contains(loadedScenes[i].name))
                        nonSystemScenes++;
                }

                return nonSystemScenes;
            }
        }

        public float Progress
        {
            get
            {
                if (_totalSteps < 1)
                {
                    Debug.LogError(GetType().Name + "Unable to calculate loading progress. Total steps can't be zero!", this);
                    return 0.0f;
                }
                    
                return (_stepsDone + _currentProgress / _totalSteps) ;
            }
        }

        public bool InProgress { get { return _routines.Count > 0 || _currentState != SceneLoaderStates.Idle; }  }

        public enum SceneLoaderStates { Idle, Loading, Unloading };
    }
}