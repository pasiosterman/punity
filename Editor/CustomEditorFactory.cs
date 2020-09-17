using UnityEditor;
using UnityEngine;

namespace PUnity.Editor
{
    public class CustomEditorFactory
    {
        [MenuItem("Assets/Create/Editor/CustomEditor", priority = 2000)]
        public static void CreateCustomEditor()
        {
            var defaultPath = "Assets";
            if (Selection.activeObject != null)
                defaultPath = AssetDatabase.GetAssetPath(Selection.activeObject);

            var path = EditorUtility.SaveFilePanelInProject("Create a new CustomEditor", "NewCustomEditor", "cs", string.Empty, defaultPath);
            if (path.Length == 0)
                return;

            var fileName = System.IO.Path.GetFileNameWithoutExtension(path);
            if (fileName.Contains(" "))
            {
                Debug.LogError("File name should not contain spaces.");
                return;
            }

            // Main template
            var contents = customEditorTemplate.Replace("%NAME%", fileName);
            System.IO.File.WriteAllText(path, contents);

            AssetDatabase.Refresh();
        }

        private static readonly string customEditorTemplate =
            @"using UnityEditor;
using UnityEngine;

namespace Forbidden
{

    [CustomEditor(typeof(FOR_TYPE))]
    public class %NAME% : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }

        public FOR_TYPE TypeTarget { get { return target as FOR_TYPE; } }
    }
}";

    }
}