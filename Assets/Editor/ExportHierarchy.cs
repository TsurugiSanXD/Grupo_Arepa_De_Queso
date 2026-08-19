using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;
using System.Text;

public static class ExportHierarchy
{
    [MenuItem("Tools/Export Hierarchy To File")]
    static void Export()
    {
        var scene = EditorSceneManager.GetActiveScene();
        var roots = scene.GetRootGameObjects();
        var sb = new StringBuilder();
        sb.AppendLine("Scene: " + scene.name);
        foreach (var root in roots)
        {
            WriteGO(root, sb, 0);
        }

        var path = Path.Combine("Assets", $"Hierarchy_{scene.name}.txt");
        File.WriteAllText(path, sb.ToString());
        Debug.Log("Exported hierarchy to " + path);
        AssetDatabase.Refresh();
    }

    static void WriteGO(GameObject go, StringBuilder sb, int indent)
    {
        sb.AppendLine(new string(' ', indent * 2) + go.name);
        foreach (Transform child in go.transform)
            WriteGO(child.gameObject, sb, indent + 1);
    }
}
