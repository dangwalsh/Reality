using UnityEngine;
using HoloToolkit.Unity.InputModule;
using System;

public class AdjustButton : MonoBehaviour, IInputClickHandler {
    [Tooltip("The GameObject to be controlled by the Button")]
    public GameObject TargetObject;

    private TransformGizmo gizmo;
    private Bounds bounds;


    private void Start() {
        this.gizmo = this.TargetObject.GetComponent<TransformGizmo>();
        this.bounds = this.gizmo.Bounds;
        SetTransformation(bounds);
    }

    private void SetTransformation(Bounds bounds) {
        this.transform.localPosition = new Vector3(0, 0, bounds.min.z - 1);
        this.bounds = bounds;
    }

    public void OnInputClicked(InputEventData eventData) {
        this.TargetObject.SetActive(!this.TargetObject.activeSelf);
    }
}
