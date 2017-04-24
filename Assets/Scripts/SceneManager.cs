using System;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class SceneManager : MonoBehaviour {
    [Tooltip("The Prefab that gets instantiated to hold a new model.")]
    public GameObject ModelPrefab;

    private string arguments = "";

    private List<GameObject> managers;
    /// <summary>
    /// Holds a collection of references to active gameobjects in scene.
    /// </summary>
    public List<GameObject> ModelManagers {
        get {
            if (this.managers == null) this.managers = new List<GameObject>();
            return this.managers;
        }
    }

    /// <summary>
    /// MonoBehaviour member
    /// </summary>
    private void Update() {
        var args = UnityEngine.WSA.Application.arguments;
        if (args == arguments || args == "") return;
        InstantiateModelManager(ExtractPath());
        RaiseImportCompletedEvent();
    }

    /// <summary>
    /// Creates an instance of a GameObject based on a Prefab
    /// </summary>
    /// <returns>a newly instantiated gameobject</returns>
    private void InstantiateModelManager(String path) {
        var prefab = Instantiate(this.ModelPrefab, Vector3.zero, Quaternion.identity, this.transform);
        this.ModelManagers.Add(prefab);
        ImportModel(path, prefab);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <param name="prefab"></param>
    private static void ImportModel(String path, GameObject prefab) {
        var manager = prefab.GetComponent<ModelManager>();
        manager.ImportModel(path);
    }

    /// <summary>
    /// removes "File=" from a string.
    /// </summary>
    /// <returns>the path</returns>
    private String ExtractPath() {
        arguments = UnityEngine.WSA.Application.arguments;
        var path = arguments.Replace("File=", "");
        return path;
    }


    #region ImportEvent Members

    public delegate void EventHandler(object sender, EventArgs args);
    public event EventHandler ImportCompleted = delegate { };

    /// <summary>
    /// Invokes the ImportCompleted event.
    /// </summary>
    private void RaiseImportCompletedEvent() {
        if (ImportCompleted != null)
            ImportCompleted(this, new EventArgs());
    }

    #endregion

    #region IHoldHandler Members [NOT USED]
    public void OnHoldStarted(HoldEventData eventData) {

    }

    public void OnHoldCompleted(HoldEventData eventData) {

    }

    public void OnHoldCanceled(HoldEventData eventData) {

    }

    #endregion
}
