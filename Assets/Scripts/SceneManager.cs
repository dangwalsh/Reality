using System;
using System.Collections;
using UnityEngine;

public class SceneManager : MonoBehaviour {
    [Tooltip("The Prefab that gets instantiated to hold a new model.")]
    public GameObject ModelManagerPrefab;

    private string args = "";

    //private void Start() {
    //    UnityEngine.WSA.Application.windowActivated += OnWindowActivated;
    //}

    //private void OnWindowActivated(WindowActivationState state) {
    //    var arguments = UnityEngine.WSA.Application.arguments;
    //}

    //private void OnApplicationFocus(bool focus) {
    //    if (focus)
    //        StartCoroutine(InstantiateModelManager(ExtractPath()));          
    //}

    /// <summary>
    /// MonoBehaviour member.
    /// </summary>
    private void Update() {
        var arguments = UnityEngine.WSA.Application.arguments;
        if (arguments == args || arguments == "") return;
        StartCoroutine(InstantiateModelManager(ExtractPath()));
    }

    /// <summary>
    /// Creates an instance of a GameObject based on a Prefab.
    /// </summary>
    /// <returns>a newly instantiated gameobject</returns>
    private IEnumerator InstantiateModelManager(String path) {
        var prefab = Instantiate(this.ModelManagerPrefab, 
                                 Vector3.zero, 
                                 Quaternion.identity, 
                                 this.transform);
        ImportModel(path, prefab);
        yield return null;
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
        args = UnityEngine.WSA.Application.arguments;
        var path = args.Replace("File=", "");
        return path;
    }
}
