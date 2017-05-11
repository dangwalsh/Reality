using System;
using UnityEngine;

public class GizmoManager : MonoBehaviour {

    public enum AxisEnum { x, y, z }

    [Tooltip("Tweaks the size of the gizmo.")]
    public float userScale = 1.0f;

    [Tooltip("The object representing the Rotate handle.")]
    public GameObject rotateGizmo;

    [Tooltip("The object representing the Scale handle.")]
    public GameObject scale1Gizmo;

    [Tooltip("The object representing the Scale handle.")]
    public GameObject scale2Gizmo;

    [Tooltip("The object representing the Translate handle.")]
    public GameObject translateGizmo;

    [Tooltip("The object representing the Remove handle.")]
    public GameObject removeGizmo;

    [Serializable]
    public struct Axis {
        public string name;
        public AxisEnum direction;
        public GameObject prefab;
    }

    [Tooltip("The axes that this gizmo has")]
    [SerializeField]
    public Axis[] Axes;

    private Vector3 size;
    private float initialPosition;

    public Bounds bounds {
        get;
        private set;
    }

    /// <summary>
    /// Called once at instantiation.
    /// </summary>
    private void Awake() {

        this.initialPosition = this.transform.position.z;
        UpdateVectors();
    }

    /// <summary>
    /// Called every frame.
    /// </summary>
    private void Update() {

        UpdateVectors();
        this.transform.localScale = this.size;
    }

    /// <summary>
    /// Generates all of the vectors needed to draw the Gizmo.
    /// </summary>
    private void UpdateVectors() {

        var baseUnit = CalculateScaleUnit() / this.initialPosition;
        this.size = new Vector3(baseUnit, baseUnit, baseUnit);
    }

    /// <summary>
    /// Returns a scalar value relating the size of the gizmo either to its location or the screen.
    /// </summary>
    /// <returns></returns>
    private float CalculateScaleUnit() {

        float scaleVector;
#if SCREEN_REL
        scaleVector = Camera.main.orthographicSize * 2.0f * Screen.width / Screen.height;
#else
        scaleVector = (Camera.main.transform.position - this.transform.position).magnitude;
#endif
        return scaleVector * this.userScale;
    }
}
