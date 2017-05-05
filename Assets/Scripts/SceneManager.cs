using System;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour {
    [Tooltip("The Prefab that gets instantiated to hold a new model.")]
    public GameObject ModelManagerPrefab;

    //private string arguments = "";

    ///// <summary>
    ///// MonoBehaviour member
    ///// </summary>
    //private void Update() {
    //    var args = UnityEngine.WSA.Application.arguments;
    //    if (args == arguments || args == "") return;
    //    InstantiateModelManager(ExtractPath());
    //}

    private void OnApplicationFocus(bool focus) {
        if (focus) {
            if (UnityEngine.WSA.Application.arguments == "") return;
            InstantiateModelManager(ExtractPath());
        }
    }

    /// <summary>
    /// Creates an instance of a GameObject based on a Prefab
    /// </summary>
    /// <returns>a newly instantiated gameobject</returns>
    private void InstantiateModelManager(String path) {
        var prefab = Instantiate(this.ModelManagerPrefab, Vector3.zero, Quaternion.identity, this.transform);
        ImportModel(path, prefab);
    }

    /// <summary>
    /// Call the import method of the instance.
    /// </summary>
    /// <param name="path"></param>
    /// <param name="prefab"></param>
    private static void ImportModel(String path, GameObject prefab) {
        var importer = prefab.GetComponent<ModelImport>();
        importer.ImportModel(path);
    }

    /// <summary>
    /// removes "File=" from a string.
    /// </summary>
    /// <returns>the path</returns>
    private String ExtractPath() {
        var arguments = UnityEngine.WSA.Application.arguments;
        var path = arguments.Replace("File=", "");
        return path;
    }
}
