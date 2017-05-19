using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class ToggleTarget : MonoBehaviour, IInputHandler {

    [Tooltip("The Gizmo object to be toggled.")]
    public GameObject targetObject;

    /// <summary>
    /// Called when AirTap gesture is started.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnInputDown(InputEventData eventData) {

        this.targetObject.SetActive(!this.targetObject.activeSelf);
    }


    /// <summary>
    /// Called when AirTap gesture is completed
    /// </summary>
    /// <param name="eventData"></param>
    public void OnInputUp(InputEventData eventData) {

    }
}
