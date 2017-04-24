using HoloToolkit.Unity.InputModule;
using UnityEngine;


public class ControlManager : MonoBehaviour, IInputClickHandler {

    [Tooltip("The Gizmo Object to be controlled")]
    public GameObject GizmoObject;

    private bool controlling;

    public void OnInputClicked(InputEventData eventData) {
        controlling = !controlling;
        this.GizmoObject.SetActive(controlling);
    }

}

