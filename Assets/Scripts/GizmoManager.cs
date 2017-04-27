using HoloToolkit.Unity.InputModule;
using UnityEngine;


public class GizmoManager: MonoBehaviour, IInputClickHandler {
    [Tooltip("The object to be toggled.")]
    public GameObject ToggledObject;

    public void OnInputClicked(InputEventData eventData) {
        //this.ToggledObject.SetActive(!this.ToggledObject.activeSelf);
    }
}

