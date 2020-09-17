using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Linq;

namespace PUnity.Editor
{
    public class HierarchyHelperWindow : EditorWindow
    {
        int tab = 0;
        Vector2 exploreScroll = Vector2.zero;
        Vector2 hierarchyScroll = Vector2.zero;

        Transform parentTransform;
        string nameToApply = "";

        [MenuItem("Window/Pooki/HierarchyHelperWindow")]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            HierarchyHelperWindow window = (HierarchyHelperWindow)EditorWindow.GetWindow(typeof(HierarchyHelperWindow));
            window.Show();
        }

        private void OnGUI()
        {
            tab = GUILayout.Toolbar(tab, new string[] { "Fix hierarchy", "Explore" });

            switch (tab)
            {
                case 0:
                    {
                        DrawFixHierarchy();
                        break;
                    }
                case 1:
                    {
                        DrawExplore();
                        break;
                    }
                default:
                    break;
            }
        }

        void DrawFixHierarchy()
        {
            exploreScroll = EditorGUILayout.BeginScrollView(exploreScroll);

            DrawReparent();
            DrawRename();

            EditorGUILayout.EndScrollView();
        }

        void DrawExplore()
        {
            exploreScroll = EditorGUILayout.BeginScrollView(exploreScroll);

            DrawShowInExplorer();
            DrawSetFilter();

            EditorGUILayout.EndScrollView();
        }

        void DrawRename()
        {
            EditorGUILayout.BeginVertical("Box");
            {
                EditorGUILayout.LabelField("Rename selected GameObjects", EditorStyles.boldLabel);
                nameToApply = EditorGUILayout.TextField(nameToApply);

                if (GUILayout.Button("Rename"))
                {
                    if (string.IsNullOrEmpty(nameToApply))
                    {
                        Debug.LogError("Unable to rename. Given name is empty or null");
                        return;
                    }

                    List<GameObject> selected = new List<GameObject>(Selection.gameObjects);
                    selected = selected.OrderBy(x => x.transform.GetSiblingIndex()).ToList();
                    string numStr = selected.Count.ToString();

                    for (int i = 0; i < selected.Count; i++)
                        selected[i].name = nameToApply + (i + 1).ToString("D" + numStr.Length);
                }
            }
            EditorGUILayout.EndVertical();
        }

        void DrawReparent()
        {
            EditorGUILayout.BeginVertical("Box");
            {
                EditorGUILayout.LabelField("Reparent selected Transforms", EditorStyles.boldLabel);

                Transform newParent = EditorGUILayout.ObjectField(parentTransform, typeof(Transform), true) as Transform;
                if (newParent != parentTransform)
                    parentTransform = newParent;

                if (GUILayout.Button("Reparent"))
                {
                    List<Transform> reparent = new List<Transform>();
                    GameObject[] arr = Selection.gameObjects;

                    //Avoid reparenting transforms that are children of other 
                    //currently selected transforms
                    for (int i = 0; i < arr.Length; i++)
                    {
                        Transform it1 = arr[i].transform;

                        bool isChild = false;
                        for (int j = 0; j < arr.Length; j++)
                        {
                            if (i == j) continue;
                            Transform it2 = arr[j].transform;

                            if (it1.IsChildOf(it2))
                            {
                                isChild = true;
                                break;
                            }
                        }

                        if (!isChild)
                        {
                            Undo.RecordObject(it1, "reparent");
                            reparent.Add(it1);
                        }
                    }

                    for (int i = 0; i < reparent.Count; i++)
                        reparent[i].SetParent(parentTransform, true);
                }
            }

            EditorGUILayout.EndVertical();
        }

        public void DrawShowInExplorer()
        {
            EditorGUILayout.BeginVertical("Box");
            {
                EditorGUILayout.LabelField("Explore", EditorStyles.boldLabel);

                if (GUILayout.Button("Persistent datapath (Save files)"))
                    EditorUtility.RevealInFinder(Application.persistentDataPath);

                if (GUILayout.Button("Data path (Project)"))
                    EditorUtility.RevealInFinder(Application.dataPath);

                if (GUILayout.Button("Application path (Unity)"))
                    EditorUtility.RevealInFinder(EditorApplication.applicationPath);
            }
            EditorGUILayout.EndVertical();
        }

        public void DrawSetFilter()
        {

            EditorGUILayout.BeginVertical("Box");
            {
                EditorGUILayout.LabelField("Filter", EditorStyles.boldLabel);

                if (GUILayout.Button("Colliders"))
                    SetSearchFilter("collider", 2);

                if (GUILayout.Button("Renderers"))
                    SetSearchFilter("Renderer", 2);
            }
            EditorGUILayout.EndVertical();
        }


        public const int FILTERMODE_ALL = 0, FILTERMODE_NAME = 1, FILTERMODE_TYPE = 2;
        public static void SetSearchFilter(string filter, int filterMode)
        {
            try
            {
                SearchableEditorWindow[] windows = (SearchableEditorWindow[])Resources.FindObjectsOfTypeAll(typeof(SearchableEditorWindow));
                SearchableEditorWindow hierarchy = null;

                foreach (SearchableEditorWindow window in windows)
                {

                    if (window.GetType().ToString() == "UnityEditor.SceneHierarchyWindow")
                    {

                        hierarchy = window;
                        break;
                    }
                }

                if (hierarchy == null)
                    return;

                MethodInfo setSearchType = typeof(SearchableEditorWindow).GetMethod("SetSearchFilter", BindingFlags.NonPublic | BindingFlags.Instance);
                object[] parameters = new object[] { filter, filterMode, false, false };

                setSearchType.Invoke(hierarchy, parameters);
            }
            catch (System.Exception e)
            {
                Debug.LogError("Failed to set search filter through reflection. Possibly due to changes within unity version. \n " + e.Message);
            }
        }
    }
}