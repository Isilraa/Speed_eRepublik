using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public class Modules : ScriptableObject {

    public GameObject[] _modules;

    [MenuItem("Assets/Create/ModulesList")]
    static void CreateTileSet()
    {
        Modules asset = ScriptableObject.CreateInstance<Modules>();
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);

        if (string.IsNullOrEmpty(path))
        {
            path = "Assets/";
        }
        else if (Path.GetExtension(path) != "")
        {
            path = path.Replace(Path.GetFileName(path), "");
        }
        else
        {
            path += "/";
        }

        var assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "ModulesList.asset");
        AssetDatabase.CreateAsset(asset, assetPathAndName);
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
        asset.hideFlags = HideFlags.DontSave;

    }
}
