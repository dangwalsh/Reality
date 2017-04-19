using System;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class SceneManager : MonoBehaviour, IInputClickHandler 
    {

    [Tooltip("The GameObject to which the scene will be attached.")]
    public GameObject SceneObject;

    [Tooltip("The Gameobject that contains the gizmo")]
    public GameObject GizmoObject;

    private string arguments = "";

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


    #region IInputClickHandler Members

    /// <summary>
    /// Handles the InputClicked event.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnInputClicked(InputEventData eventData) {
        var gizmo = GameObject.Find("Gizmo");
        gizmo.SetActive(!gizmo.activeSelf);
    }

    #endregion

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
}
