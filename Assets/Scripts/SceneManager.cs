using System;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using System.Linq;

public class SceneManager : MonoBehaviour, IHoldHandler
    {

    [Tooltip("The GameObject to which the scene will be attached.")]
    public GameObject SceneObject;

    [Tooltip("The Gameobject that contains the gizmo")]
    public GameObject GizmoObject;

    private string arguments = "";
    private GameObject gizmo;


    private void Start() {
        var transforms = this.GetComponentsInChildren(typeof(Transform), true);
        var gizmoTransform = transforms.FirstOrDefault(t => t.name == "Gizmo");
        gizmo = gizmoTransform.gameObject;
    }

    private void Update() {
        if (UnityEngine.WSA.Application.arguments == arguments || 
            UnityEngine.WSA.Application.arguments == "") return;
        arguments = UnityEngine.WSA.Application.arguments;
        var path = arguments.Replace("File=", "");
#if !UNITY_EDITOR
        float size = Geometry.Initialize(path, this.SceneObject, this.transform);
        RaiseImportCompletedEvent();
        this.transform.localScale /= (size / 5); 
        this.transform.position += new Vector3(0, 0, 20);
#endif
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

    #region IHoldHandler Members

    public void OnHoldStarted(HoldEventData eventData) {
        gizmo.SetActive(!gizmo.activeSelf);
    }

    public void OnHoldCompleted(HoldEventData eventData) {
        
    }

    public void OnHoldCanceled(HoldEventData eventData) {

    }

    #endregion
}
